using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml.Schema;
using System.Xml.Linq;

namespace UIRibbonTools
{
    class BuildPreviewHelper
    {
        public const string Neutral = "Neutral";

        //private Action<bool> _buildActionEnabled;
        private Action<bool> _previewActionEnabled;
        //private Action<string> _setText;
        private Action<IList<string>> _setLanguages;
        private string _uiccXsdPath = string.Empty;
        private string _selectedCulture = Neutral;
        private Action<MessageKind, string> _log;

        public bool HasValidParser
        {
            get { return (Parser != null); }
            set
            {
                if (!value)
                    Parser = null;
                else if (Parser == null)
                    Parser = new RibbonParser(XmlRibbonFile);
            }
        }

        public static BuildPreviewHelper Instance = new BuildPreviewHelper();

        private BuildPreviewHelper()
        {
            //string sdkPath = null;
            //sdkPath = Util.DetectAppropriateWindowsSdkPath();
            //_uiccXsdPath = Path.Combine(sdkPath, "UICC.xsd");
        }

        public void ShowPreviewDialog(Form form)
        {
            if (Parser == null)
                Parser = new RibbonParser(XmlRibbonFile);
            SetPreviewUiCulture();
            PreviewForm dialog = new PreviewForm();
            dialog.ShowDialog(form);
            ResetPreviewUiCulture();
        }

        private void SetPreviewUiCulture()
        {
            if (_selectedCulture.ToUpperInvariant().Equals(Neutral.ToUpperInvariant()))
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            else
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(_selectedCulture);
        }

        private void ResetPreviewUiCulture()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture;
        }

        public string UICulture
        {
            get { return _selectedCulture; }
            set { _selectedCulture = value; }
        }

        public RibbonParser Parser { get; private set; }

        /// <summary>
        /// Name parameter used by UICC.exe
        /// </summary>
        public string ResourceIdentifier { get; set; }

        public void SetActions(Action<bool> buildActionEnabled, Action<bool> previewActionEnabled, Action<MessageKind, string> log
            , Action<IList<string>> setLanguages)
        {
            //this._buildActionEnabled = buildActionEnabled;
            this._previewActionEnabled = previewActionEnabled;
            this._log = log;
            this._setLanguages = setLanguages;
            //buildActionEnabled(false);
            //previewActionEnabled(false);
        }

        public void SetRibbonXmlFile(string path)
        {
            string validateMsg = null;
            //bool buildEnabled = false;
            bool previewEnabled = false;
            path = Path.GetFullPath(path);
            if (File.Exists(path))
            {
                //validateMsg = Validation(path);
                bool validate = (string.IsNullOrEmpty(validateMsg) ? true : false);
                if (validate)
                {
                    XmlRibbonFile = path;
                    HasValidParser = false;
                    //buildEnabled = true;
                    _setLanguages(FindLanguages(path));
                    _selectedCulture = BuildPreviewHelper.Neutral;
                    previewEnabled = CheckMarkupResource(path);
                }
                else
                {
                    _log(MessageKind.Pipe, validateMsg);
                    MessageBox.Show("Xml is not valid", "XML Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //buildActionEnabled(buildEnabled);
            _previewActionEnabled(previewEnabled);
        }

        private static IList<string> FindLanguages(string path)
        {
            string directory = Path.GetDirectoryName(path);
            string markup = Path.GetFileNameWithoutExtension(path);
            string[] files = Directory.GetFiles(directory, markup + ".*.resx", SearchOption.TopDirectoryOnly);
            List<string> languages = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                string languageMarkup = Path.GetFileNameWithoutExtension(files[i]);
                int index = languageMarkup.LastIndexOf('.');
                if (index >= 0)
                {
                    languages.Add(languageMarkup.Substring(index + 1));
                }
            }
            return languages;
        }

        private bool CheckMarkupResource(string path)
        {
            bool previewEnabled = false;
            string markupResourceFileName = Path.ChangeExtension(path, "ribbon");
            if (File.Exists(markupResourceFileName))
            {
                SetMarkupResource(markupResourceFileName);
                previewEnabled = true;
                if (Parser == null || !Parser.Results.HasHFile)
                    Parser = new RibbonParser(path);
            }
            return previewEnabled;
        }

        private void SetMarkupResource(string path)
        {
            MarkupResource = "file://" + path;
        }

        /// <summary>
        /// MainForm sets the name, PreviewForm uses it for RibbonStrip.MarkupResource
        /// MarkupResource starts with "file://". It's a file based resource
        /// </summary>
        public string MarkupResource { get; private set; }

        public string XmlRibbonFile { get; private set; }

        private string Validation(string path)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("http://schemas.microsoft.com/windows/2009/Ribbon", _uiccXsdPath);

            XDocument doc = XDocument.Load(path);
            string msg = string.Empty;
            doc.Validate(schemas, (sender, e) =>
            {
                msg += e.Message + Environment.NewLine;
            });
            return msg;
            //Console.WriteLine(msg == "" ? "Document is valid" : "Document invalid: " + msg);
        }

        public void BuildRibbonFile(string resourceIdentifier)
        {
            MessageOutput message = null;
            try
            {
                string path = XmlRibbonFile;
                string content = File.ReadAllText(path);
                Manager manager = new Manager(message = new MessageOutput(), path, content);

                var targets = manager.Targets;
                foreach (var target in targets)
                {
                    var buffer = manager.CreateRibbon(target, resourceIdentifier);
                    File.WriteAllBytes(target.RibbonFilename, buffer);
                }
                bool validResource = CheckMarkupResource(path);
                _previewActionEnabled(validResource);

                // create the C#, VB file RibbonItems.Designer.cs
                if (validResource)
                    CodeWrapperBuilder(path, Parser);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (message != null)
                {
                    string allMsg = message.GetString();
                    _log(MessageKind.Pipe, allMsg);
                    BuildLogFile(XmlRibbonFile, allMsg);
                    message.Close();
                }
            }
        }

        private static void BuildLogFile(string fileName, string logging)
        {
            StringReader sr = new StringReader(logging);
            StreamWriter sw = File.CreateText(Path.ChangeExtension(fileName, "log"));
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                sw.WriteLine(line);
            }
            sw.Close();
            sr.Close();
        }

        private static void CodeWrapperBuilder(string path, RibbonParser parser)
        {
            bool advWrapperClassFile = Settings.Instance.AdvancedWrapperClassFile;
            if (Settings.Instance.BuildCSharpWrapper)
                new CSharpCodeBuilder().Execute(path, parser, advWrapperClassFile);
            if (Settings.Instance.BuildVBWrapper)
                new VBCodeBuilder().Execute(path, parser, advWrapperClassFile);
        }

        public static void ConsoleBuild(string path, string resourceIdentifier)
        {
            ConsoleMessageOutput message = null;
            try
            {
                string fileName = path;
                string content = File.ReadAllText(fileName);

                Manager manager = new Manager(message = new ConsoleMessageOutput(), fileName, content);

                var targets = manager.Targets;
                foreach (var target in targets)
                {
                    var buffer = manager.CreateRibbon(target, resourceIdentifier);
                    File.WriteAllBytes(target.RibbonFilename, buffer);
                }

                // create the C#, VB file RibbonItems.Designer.cs
                CodeWrapperBuilder(fileName, new RibbonParser(fileName));

            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.Message);
            }
            finally
            {
                if (message != null)
                {
                    BuildLogFile(path, message.GetString());
                    message.Close();
                }
            }
        }
    }
}
