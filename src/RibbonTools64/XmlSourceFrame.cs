#define xele
//#define xtext
//#define XmlChars

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Controls;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

namespace UIRibbonTools
{
    partial class XmlSourceFrame : UserControl
    {
        //Stopwatch watch = new Stopwatch();

        private const DRAW_TEXT_FORMAT TEXT_FLAGS = DRAW_TEXT_FORMAT.DT_NOPREFIX | DRAW_TEXT_FORMAT.DT_NOCLIP | DRAW_TEXT_FORMAT.DT_SINGLELINE;
        private const int COLOR_FOCUS_RGB = unchecked((int)0xFFFFFFC0);
        private const int COLOR_MARGIN_RGB = unchecked((int)0xffF4F4F4);
        private const int COLOR_MARGIN_TEXT_RGB = unchecked((int)0xff9999CC);
        private const int COLOR_SYMBOL_RGB = unchecked((int)0xff0000FF);
        private const int COLOR_ELEMENT_RGB = unchecked((int)0xffA31515);
        private const int COLOR_ATTRIBUTE_RGB = unchecked((int)0xffFF0000);
        private const int COLOR_CONTENT_RGB = unchecked((int)0xff000000);

        private readonly COLORREF COLOR_FOCUS = (COLORREF)0xC0FFFF;
        private readonly COLORREF COLOR_MARGIN = (COLORREF)0xF4F4F4;
        private readonly COLORREF COLOR_MARGIN_TEXT = (COLORREF)0xCC9999;
        private readonly COLORREF COLOR_SYMBOL = (COLORREF)0xFF0000;
        private readonly COLORREF COLOR_ELEMENT = (COLORREF)0x1515A3;
        private readonly COLORREF COLOR_ATTRIBUTE = (COLORREF)0x0000FF;
        private readonly COLORREF COLOR_CONTENT = (COLORREF)0x000000;

        private TRibbonDocument _document;
        private XDocument _xmlDoc;
        private int _marginWidth;
        private int _spaceWidth;
        private int _equalWidth;
        private int _quoteWidth;
        private int _lessThanSlashWidth;
        private int _greaterThanWidth;
        private int _lineCount;
        private bool _allowExpandCollapse;
        private List<TreeNode> _treeNodes;
        Action<bool> ForceScrollbarUpdate;

#if xtext
        private List<string> _xmlFile;
#endif
        private Pen _marginTextPen = new Pen(Color.FromArgb(COLOR_MARGIN_TEXT_RGB));
        private Brush _marginBrush = new SolidBrush(Color.FromArgb(COLOR_MARGIN_RGB));
        private Brush _focusBrush = new SolidBrush(Color.FromArgb(COLOR_FOCUS_RGB));

        //public static void SetDoubleBuffered(Control control)
        //{
        //    // set instance non-public property with name "DoubleBuffered" to true
        //    typeof(Control).InvokeMember("DoubleBuffered",
        //        BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
        //        null, control, new object[] { true });
        //}

        public XmlSourceFrame()
        {
            InitializeComponent();
#if xele
            treeViewXmlSource.NodeMouseClick += TreeViewXmlSourceClick;
            treeViewXmlSource.NodeMouseDoubleClick += TreeViewXmlSourceDblClick;
            treeViewXmlSource.BeforeCollapse += TreeViewXmlSourceCollapsing;
            treeViewXmlSource.BeforeExpand += TreeViewXmlSourceExpanding;

            treeViewXmlSource.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            treeViewXmlSource.DrawNode += TreeViewXmlSourceCustomDrawItemX2;
#endif
            MethodInfo method = treeViewXmlSource.GetType().GetMethod("ForceScrollbarUpdate", BindingFlags.Instance | BindingFlags.NonPublic);
            //method.Invoke(treeViewXmlSource, new object[] { false });
            ForceScrollbarUpdate = method.CreateDelegate<Action<bool>>(treeViewXmlSource);
            //ForceScrollbarUpdate(false);
        }

        public void SetFonts(Font font)
        {
            this.Font = font;
        }

#if xele
        private TreeNode AddNode(TreeNode parent, XElement element,
            ref int lineNum)
        {
            TreeNode result;
            TreeNode endNode;

            string text = string.Empty;
            if (parent != null)
            {
                result = parent.Nodes.Add(text);
            }
            else
            {
                result = new TreeNode(text);
                _treeNodes.Add(result);
            }
            result.Tag = element;
            result.ImageIndex = lineNum;
            lineNum++;
            if (element.Nodes().Count() > 0)
            {
                bool isXmlElement = false;
                foreach (XNode child in element.Nodes())
                {
                    XElement childElement = child as XElement;
                    if (childElement != null)
                    {
                        isXmlElement = true;
                        AddNode(result, childElement, ref lineNum);
                    }
                }

                if (isXmlElement)
                {
                    text = element.Name.LocalName;
                    if (parent != null)
                    {
                        endNode = parent.Nodes.Add(text);
                    }
                    else
                    {
                        endNode = new TreeNode(text);
                        _treeNodes.Add(endNode);
                    }
                    endNode.ImageIndex = lineNum;
                    lineNum++;
                }
            }
            return result;
        }
#endif

#if xtext
        private TreeNode AddNodeT(TreeNode parent, XElement element,
            ref int lineNum)
        {
            TreeNode result;
            TreeNode endNode;
            string text;
            text = _xmlFile[lineNum - 1];
            if (parent != null)
            {
                result = parent.Nodes.Add(text);
            }
            else
            {
                result = new TreeNode(text);
                _treeNodes.Add(result);
                //treeViewXmlSource.Nodes.Add(result);
            }
            result.Tag = element;
            result.ImageIndex = lineNum;
            lineNum++;
            if (element.Nodes().Count() > 0)
            {
                bool isXmlElement = false;
                foreach (XNode child in element.Nodes())
                {
                    XElement childElement = child as XElement;
                    if (childElement != null)
                    {
                        isXmlElement = true;
                        AddNodeT(result, childElement, ref lineNum);
                    }
                }

                if (isXmlElement)
                {
                    text = _xmlFile[lineNum - 1];
                    if (parent != null)
                    {
                        endNode = parent.Nodes.Add(text);
                    }
                    else
                    {
                        endNode = new TreeNode(text);
                        _treeNodes.Add(endNode);
                    }
                    endNode.Tag = element.Name;
                    endNode.ImageIndex = lineNum;
                    lineNum++;
                }
            }
            return result;
        }
#endif

        public void ClearDocument()
        {
            treeViewXmlSource.Nodes.Clear();
        }

        public void ShowDocument(TRibbonDocument document)
        {
            _document = document;
        }

        public void ActivateFrame()
        {
            _treeNodes = new List<TreeNode>();
            TreeNode root = null;
            int lineNum;
            _marginWidth = -1;
            treeViewXmlSource.Visible = false;
            //watch.Restart();
            treeViewXmlSource.Nodes.Clear();
            treeViewXmlSource.BeginUpdate();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if ((_document != null) && File.Exists(_document.Filename))
                {
                    _xmlDoc = XDocument.Load(_document.Filename);
#if xtext
                    MemoryStream stream = new MemoryStream();
                    _xmlDoc.Save(stream);
                    stream.Position = 0;

                    StreamReader sr = new StreamReader(stream);
                    _xmlFile = new List<string>();
                    while (!sr.EndOfStream)
                        _xmlFile.Add(sr.ReadLine().Trim());
                    sr.Close();
#endif

                    lineNum = 2;
#if xele
                    root = AddNode(null, _xmlDoc.Root, ref lineNum);
#endif
#if xtext
                    root = AddNodeT(null, _xmlDoc.Root, ref lineNum);
#endif
                    treeViewXmlSource.Nodes.AddRange(_treeNodes.ToArray());
                    _lineCount = lineNum - 1;
                    _allowExpandCollapse = true;
                    treeViewXmlSource.SelectedNode = root;
                    treeViewXmlSource.ExpandAll();
                    //root.EnsureVisible();
                }
            }
            finally
            {
                treeViewXmlSource.EndUpdate();
                treeViewXmlSource.Visible = true;
                //watch.Stop();
                treeViewXmlSource.Select();
                _allowExpandCollapse = false;
                root.EnsureVisible();
                this.Cursor = Cursors.Default;
            }
        }

        public void DeactivateFrame()
        {
            //Nothing yet
        }

        private void TreeViewXmlSourceCollapsing(object sender,
            TreeViewCancelEventArgs e)
        {
            e.Cancel = !_allowExpandCollapse;
        }

        private void TreeViewXmlSourceClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;

            if (node != null)
                treeViewXmlSource.SelectedNode = node;

            if ((node != null) && (node.Nodes.Count > 0) && (e.X < _marginWidth) && (e.X > (_marginWidth - 10)))
            {
                _allowExpandCollapse = true;
                try
                {
                    if (!node.IsExpanded)
                    {
                        node.ExpandAll();
                    }
                    else
                        node.Collapse(true);
                }
                finally
                {
                    _allowExpandCollapse = false;
                }
            }
        }

        private void TreeViewXmlSourceDblClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if ((node != null) && (node.Nodes.Count > 0))
            {
                _allowExpandCollapse = true;
                try
                {
                    if (!node.IsExpanded)
                    {
                        node.ExpandAll();
                    }
                    else
                        node.Collapse(true);
                }
                finally
                {
                    _allowExpandCollapse = false;
                }
            }
        }

        private void TreeViewXmlSourceExpanding(object sender,
            TreeViewCancelEventArgs e)
        {
            e.Cancel = !_allowExpandCollapse;
        }

        private unsafe void UpdateNode(TreeNode node, string text)
        {
            TVITEMW item = new TVITEMW()
            {
                hItem = (HTREEITEM)node.Handle,
                mask = TVITEM_MASK.TVIF_HANDLE | TVITEM_MASK.TVIF_TEXT
            };
            fixed (char* textLocal = text)
            {
                item.pszText = new PWSTR(textLocal);
                PInvoke.SendMessage((HWND)treeViewXmlSource.Handle, PInvoke.TVM_SETITEMW, (WPARAM)0, (LPARAM)(IntPtr)(void*)&item);
            }
            //Marshal.FreeHGlobal((IntPtr)item.pszText.Value);
            if (treeViewXmlSource.Scrollable)
            {
                ForceScrollbarUpdate(false);
            }
        }

        /// <summary>
        /// CustomDraw with the most native functions (DrawText for calculation of text length)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private unsafe void TreeViewXmlSourceCustomDrawItemX2(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Bounds.Height < 1 || e.Bounds.Width < 1)
                return;
            TreeView view = (TreeView)sender;
            if (e.Bounds.Y > view.Height)
                return;
            //former code speed up the OwnerDraw TreeView

            RECT rN, rT, rM;
            HDC dC; //HDC
            XElement element;
            string s;
            int i;
            //Font nodeFont;
            Size proposedSize = new Size(int.MaxValue, int.MaxValue);
            //TextFormatFlags flags = TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;
            //nodeFont = e.Node.NodeFont;
            //if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

            element = e.Node.Tag as XElement;
            dC = (HDC)e.Graphics.GetHdc();
            PInvoke.SetBkMode(dC, BACKGROUND_MODE.TRANSPARENT);

            rM = new RECT();
            rN = new RECT(e.Bounds); //Node.DisplayRect(false);
            rT = new RECT(Rectangle.Inflate(e.Bounds, -e.Node.Bounds.X + 19, 0)); // onlyText
            rT.right = rN.right;

            if (_marginWidth < 0)
            {
                s = _lineCount.ToString();
                char[] ch = new char[s.Length];
                for (i = 0; i < s.Length; i++)
                    ch[i] = '0';
                s = new string(ch);

                fixed (char* p = s)
                    PInvoke.DrawText(dC, p, s.Length, ref rM, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_CALCRECT);
                _marginWidth = rM.Size.Width + 19;
                fixed (char* p = " ")
                    PInvoke.DrawText(dC, p, 1, ref rM, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_CALCRECT);
                _spaceWidth = rM.Size.Width;
                fixed (char* p = "=")
                    PInvoke.DrawText(dC, p, 1, ref rM, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_CALCRECT);
                _equalWidth = rM.Size.Width;
                fixed (char* p = "\"")
                    PInvoke.DrawText(dC, p, 1, ref rM, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_CALCRECT);
                _quoteWidth = rM.Size.Width;
                fixed (char* p = "</")
                    PInvoke.DrawText(dC, p, 2, ref rM, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_CALCRECT);
                _lessThanSlashWidth = rM.Size.Width;
                fixed (char* p = ">")
                    PInvoke.DrawText(dC, p, 1, ref rM, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_CALCRECT);
                _greaterThanWidth = rM.Size.Width;
            }
            e.Graphics.ReleaseHdc(dC);

            // Focus rect
            if ((e.State & TreeNodeStates.Focused) != 0)
            {
                e.Graphics.FillRectangle(_focusBrush, (Rectangle)rN);
            }

            // Margin with line numbers
            rN.right = rN.left + _marginWidth;
            e.Graphics.FillRectangle(_marginBrush, (Rectangle)rN);
            i = e.Node.ImageIndex;
            if (((i % 10) == 0) || ((e.State & TreeNodeStates.Focused) != 0))
                s = e.Node.ImageIndex.ToString();
            else
                s = "-";

            dC = (HDC)e.Graphics.GetHdc();
            PInvoke.SetTextColor(dC, COLOR_MARGIN_TEXT);
            rN.right -= 18;
            fixed (char* p = s)
                PInvoke.DrawText(dC, p, s.Length, ref rN, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_RIGHT);
            e.Graphics.ReleaseHdc(dC);

            // Expand / Collapse markers
            if (e.Node.Nodes.Count > 0)
            {
                e.Node.Checked = true;
                rN.left = rN.right + 6;
                rN.top = rN.top + (rN.bottom - rN.top - 9) / 2;
                rN.right = rN.left + 9;
                rN.bottom = rN.top + 9;
                e.Graphics.DrawRectangle(_marginTextPen, (Rectangle)rN);
                e.Graphics.DrawLine(_marginTextPen, rN.left + 2, rN.top + 4, rN.left + 7, rN.top + 4);
                if (!e.Node.IsExpanded)
                {
                    e.Graphics.DrawLine(_marginTextPen, rN.left + 4, rN.top + 2, rN.left + 4, rN.top + 7);
                }
            }
            else
            {
                rN.left = rN.right + 10;
                e.Graphics.DrawLine(_marginTextPen, rN.left, rN.top, rN.left, rN.bottom);
            }

            // Draw element
            dC = (HDC)e.Graphics.GetHdc();
            PInvoke.SetTextColor(dC, COLOR_SYMBOL);
            rT.left += _marginWidth;
            int width;
            if ((element != null))
            {
                s = "<";
                width = _greaterThanWidth;
            }
            else
            {
                s = "</";
                width = _lessThanSlashWidth;
            }
            fixed (char* p = s)
                PInvoke.DrawText(dC, p, s.Length, ref rT, TEXT_FLAGS);
            rT.left += width;

            PInvoke.SetTextColor(dC, COLOR_ELEMENT);
            if (element != null)
                s = element.Name.LocalName;
            else
                s = e.Node.Text;
            fixed (char* p = s)
                PInvoke.DrawText(dC, p, s.Length, ref rT, TEXT_FLAGS);

            fixed (char* p = s)
                PInvoke.DrawText(dC, p, s.Length, ref rM, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_CALCRECT);
            rT.left += rM.right - rM.left;

            // Draw attributes
            if (element != null)
            {
                foreach (XAttribute attr in element.Attributes())
                {
                    rT.left += _spaceWidth;
                    PInvoke.SetTextColor(dC, COLOR_ATTRIBUTE);
                    s = attr.Name.LocalName;
                    fixed (char* p = s)
                        PInvoke.DrawText(dC, p, s.Length, ref rT, TEXT_FLAGS);

                    fixed (char* p = s)
                        PInvoke.DrawText(dC, p, s.Length, ref rM, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_CALCRECT);
                    rT.left += rM.right - rM.left;

                    PInvoke.SetTextColor(dC, COLOR_SYMBOL);
                    fixed (char* p = "=")
                        PInvoke.DrawText(dC, p, 1, ref rT, TEXT_FLAGS);
                    rT.left += _equalWidth;

                    PInvoke.SetTextColor(dC, COLOR_CONTENT);
                    fixed (char* p = "\"")
                        PInvoke.DrawText(dC, p, 1, ref rT, TEXT_FLAGS);

                    rT.left += _quoteWidth;

                    PInvoke.SetTextColor(dC, COLOR_SYMBOL);
                    s = attr.Value;
#if XmlChars
                    s = System.Net.WebUtility.HtmlEncode(s);
                    s = s.Replace(((char)0xA).ToString(), @"&#xA;");
#else
                    s = s.Replace(((char)0xA).ToString(), @"\n");
#endif
                    fixed (char* p = s)
                        PInvoke.DrawText(dC, p, s.Length, ref rT, TEXT_FLAGS);

                    fixed (char* p = s)
                        PInvoke.DrawText(dC, p, s.Length, ref rM, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_CALCRECT);
                    rT.left += rM.right - rM.left;

                    PInvoke.SetTextColor(dC, COLOR_CONTENT);
                    fixed (char* p = "\"")
                        PInvoke.DrawText(dC, p, 1, ref rT, TEXT_FLAGS);

                    rT.left += _quoteWidth;
                }
            }

            if (element != null && element.Nodes().Count() == 1 && element.FirstNode is XText)
                s = element.Value;
            else
                s = string.Empty;

            if (!string.IsNullOrEmpty(s))
            {
#if XmlChars
                s = System.Net.WebUtility.HtmlEncode(s);
                s = s.Replace(((char)0xA).ToString(), @"&#xA;");
#else
                s = s.Replace(((char)0xA).ToString(), @"\n");
#endif
                PInvoke.SetTextColor(dC, COLOR_SYMBOL);
                fixed (char* p = ">")
                    PInvoke.DrawText(dC, p, 1, ref rT, TEXT_FLAGS);
                rT.left += _greaterThanWidth;

                PInvoke.SetTextColor(dC, COLOR_CONTENT);
                fixed (char* p = s)
                    PInvoke.DrawText(dC, p, s.Length, ref rT, TEXT_FLAGS);

                fixed (char* p = s)
                    PInvoke.DrawText(dC, p, s.Length, ref rM, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_CALCRECT);
                rT.left += rM.right - rM.left;

                PInvoke.SetTextColor(dC, COLOR_SYMBOL);
                fixed (char* p = "</")
                    PInvoke.DrawText(dC, p, 2, ref rT, TEXT_FLAGS);
                rT.left += _lessThanSlashWidth;

                s = element.Name.LocalName;
                PInvoke.SetTextColor(dC, COLOR_ELEMENT);
                fixed (char* p = s)
                    PInvoke.DrawText(dC, p, s.Length, ref rT, TEXT_FLAGS);

                fixed (char* p = s)
                    PInvoke.DrawText(dC, p, s.Length, ref rM, TEXT_FLAGS | DRAW_TEXT_FORMAT.DT_CALCRECT);
                rT.left += rM.right - rM.left;

                PInvoke.SetTextColor(dC, COLOR_SYMBOL);
                fixed (char* p = ">")
                    PInvoke.DrawText(dC, p, 1, ref rT, TEXT_FLAGS);
            }
            else
            {
                PInvoke.SetTextColor(dC, COLOR_SYMBOL);
                if ((element != null) && (e.Node.Nodes.Count == 0))
                    s = "/>";
                else
                    s = ">";
                fixed (char* p = s)
                    PInvoke.DrawText(dC, p, s.Length, ref rT, TEXT_FLAGS);
            }
            e.Graphics.ReleaseHdc(dC);
            e.DrawDefault = false;
        }
    }
}
