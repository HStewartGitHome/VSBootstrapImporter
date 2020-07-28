namespace VSBootstrapImporter
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using VSBootstrapImporter.Common.Models;
    using VSBootstrapImporter.Common.Services;

    /// <summary>
    /// Interaction logic for BootstrapImportControl.
    /// </summary>
    public partial class BootstrapImportControl : UserControl
    {
        #region variables

        public Type_Options CurrentProjectType = Type_Options.Unknown;
        public Options CurrentOptions { get; set; }
        public bool IsBootstrapHtml { get; set; }
        public string LoadedProject { get; set; }
        public string MsgTitle { get; set; }
        public DlgSupport Support { get; set; }

        #endregion variables

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapImportControl"/> class.
        /// </summary>
        public BootstrapImportControl()
        {
            this.InitializeComponent();

            Window parent = Window.GetWindow(this);

            IsBootstrapHtml = false;
            MsgTitle = "Bootstrap Importer";
            CurrentOptions = new Options();
            InitializeControls();
            Support = new DlgSupport(parent, MsgTitle);
        }

        #endregion Construction

        #region External dependency property

        public static readonly DependencyProperty ProjectPathProperty =
               DependencyProperty.Register("ProjectPath",
                                   typeof(string),
                                   typeof(BootstrapImportControl),
                                   new PropertyMetadata("", new PropertyChangedCallback(OnSetProjectPathChanged)));

        public string ProjectPath
        {
            get { return (string)GetValue(ProjectPathProperty); }
            set { SetValue(ProjectPathProperty, value); }
        }

        private static void OnSetProjectPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BootstrapImportControl UserControl1Control = d as BootstrapImportControl;
            UserControl1Control.OnSetProjectPathChanged(e);
        }

        private void OnSetProjectPathChanged(DependencyPropertyChangedEventArgs e)
        {
            string str = e.NewValue.ToString();

            if (string.IsNullOrEmpty(str) == false)
            {
                LoadedProject = str;
                HandleSelect(str);
            }
        }

        #endregion External dependency property

        #region Handle Button functions

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            HandleOk();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            HandleCreate();
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            HandleDetails();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            HandleEdit();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            HandleLoad();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            HandleNew();
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            HandlePreview();
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            HandleSelect("");
        }

        #endregion Handle Button functions

        #region Button Handle Methods

        private void HandleOk()
        {
            if (MessageBox.Show("Do you wish to exit?", MsgTitle, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Window.GetWindow(this).Close();
            }
        }

        // screen button support
        private void HandleCreate()
        {
            string str;
            if (CurrentOptions.IsReady() == false)
            {
                MessageBox.Show("Make sure all options are ready", MsgTitle);
            }
            else
            {
                if (CheckCSSIsolation() == true)
                {
                    DataInfo info = GenerateData(false, true);
                    CurrentProjectType = info.ProjectType;
                    switch (CurrentProjectType)
                    {
                        case Type_Options.BlazorServer:
                        case Type_Options.BlazorWebassembly:
                        case Type_Options.BlazorWebassemblyPWA:
                        case Type_Options.ASPNetRazor:
                            str = Options.GetProjectTypeString(CurrentProjectType);
                            MessageBox.Show(str + " Modifications created", MsgTitle);
                            break;

                        default:
                            MessageBox.Show("Unknown Project Type", MsgTitle);
                            break;
                    }
                }
            }
        }

        private void HandleDetails()
        {
            Generator theGenerator = new Generator();
            UpdateOptionsData();

            DataInfo info = theGenerator.CreateDataInfo(CurrentOptions);
            info.IsBootstrapHtml = IsBootstrapHtml;
            Details dlg = new Details(info, CurrentOptions);
            dlg.ShowDialog();
        }

        private void HandleEdit()
        {
            if (CurrentOptions.IsReady() == false)
            {
                MessageBox.Show("Make sure all options are ready", MsgTitle);
            }
            else
            {
                UpdateOptionsData();
                string str = Html.Text;

                if (Support.IsHtmlAllowed(CurrentOptions, CurrentOptions.BootstrapProjectPath, str) == false)
                {
                    if (CurrentOptions.IsTraceOn(Trace_Options.TraceWarning) == true)
                        Trace.TraceWarning("Html is not allowed with editing Html Source");
                }
                else if (MessageBox.Show("Do you wish allow editing of Html source?", MsgTitle, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    IsBootstrapHtml = Support.IsBootstrapHtml;
                    CurrentOptions.AllowHtmlEditing = true;
                    Html.IsEnabled = true;
                    IndexPage.IsEnabled = true;

                    if (MessageBox.Show("Do you wish to use Page as reference?", MsgTitle, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        str = str.Replace("Test", CurrentOptions.PageName);
                        Html.Text = str;
                    }
                }
            }
        }

        private void HandleLoad()
        {
            string str = Support.GetProjectDirectory("Bootstrap HTML Project");

            if (string.IsNullOrEmpty(str) == true)
            {
                // user hit cancel do nothing
            }
            else if (Support.IsHtmlAllowed(CurrentOptions, str, CurrentOptions.BootstrapHtml) == false)
            {
                if (CurrentOptions.IsTraceOn(Trace_Options.TraceWarning) == true)
                    Trace.TraceWarning("Html is not allowed when loading new  Html Source");
            }
            else
            {
                IsBootstrapHtml = Support.IsBootstrapHtml;
                CurrentOptions.BootstrapProjectPath = str;
                CurrentOptions.PageName = "Test";
                UpdateScreen();
            }
        }

        private void HandleNew()
        {
            if (CurrentOptions.IsReady() == false)
            {
                MessageBox.Show("Make sure all options are ready", MsgTitle);
            }
            else
            {
                if (MessageBox.Show("Do you wish start new?", MsgTitle, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ResetOptions();
                    IsBootstrapHtml = false;
                    CurrentOptions = new Options();
                    Html.IsEnabled = false; ;
                    IndexPage.IsEnabled = false;
                    UpdateScreen();
                }
            }
        }

        private void HandlePreview()
        {
            if (CurrentOptions.IsReady() == false)
            {
                MessageBox.Show("Make sure all options are ready", MsgTitle);
            }
            else
            {
                GenerateData(true, true);

                Shell.Start("notepad.exe", CurrentOptions.PreviewFile);
            }
        }

        private void HandleSelect(string strProject)
        {
            string str = strProject;
            string strCurrent = CurrentOptions.BlazorProjectPath;
            if (string.IsNullOrEmpty(str) == true)
                str = Support.GetProjectDirectory("Visual Studio Project");
            CurrentOptions.BlazorProjectPath = str;

            DataInfo info = GenerateData(true, false);
            CurrentProjectType = info.ProjectType;

            if (info.IsWebAssembly() == true)
            {
                if (SetOptions() == false)
                {
                    if (LoadedProject.Length > 0)
                        CurrentOptions.BlazorProjectPath = LoadedProject;
                    else
                        CurrentOptions.BlazorProjectPath = strCurrent;

                    info = GenerateData(true, false);
                    CurrentProjectType = info.ProjectType;
                }
            }
            UpdateScreen();
        }

        #endregion Button Handle Methods

        #region Screen depended support

        // Screen depended support functionality
        private void InitializeControls()
        {
            // initialize Modify Options
            List<ComboData> ListData = new List<ComboData>
            {
                new ComboData { Id = (int)Modify_Options.ModifyProject, Value = "Modify Project" },
                new ComboData { Id = (int)Modify_Options.ModifyNoPreview, Value = "Modify No Preview" },
                new ComboData { Id = (int)Modify_Options.PreviewOnly, Value = "Preview Only" }
            };

            ModifyOptions.ItemsSource = ListData;
            ModifyOptions.DisplayMemberPath = "Value";
            ModifyOptions.SelectedValuePath = "Id";

            // initialize Asset Options
            ListData = new List<ComboData>
            {
                new ComboData { Id = (int)Asset_Options.Static, Value = "Static" },
                new ComboData { Id = (int)Asset_Options.StaticSameIndexPage, Value = "Static Same Index Page" },
                new ComboData { Id = (int)Asset_Options.SharedPages, Value = "Share between pages" },
                new ComboData { Id = (int)Asset_Options.StaticMove, Value = "Static Move only Assets" },
                new ComboData { Id = (int)Asset_Options.StaticSameIndexMove, Value = "Static Move only Assets on Same Index Page" },
                new ComboData { Id = (int)Asset_Options.SharedPagesMove, Value = "Move and Share between pages" },
                new ComboData { Id = (int)Asset_Options.None, Value = "None" }
            };

            AssetOptions.ItemsSource = ListData;
            AssetOptions.DisplayMemberPath = "Value";
            AssetOptions.SelectedValuePath = "Id";

            // initialize CSS Options
            ListData = new List<ComboData>
            {
                new ComboData { Id = (int)CSS_Options.IncludeHttpsFonts, Value = "Include Https Fonts" },
                new ComboData { Id = (int)CSS_Options.IncludeBootstrap, Value = "Include custom Bootstrap" },
                new ComboData { Id = (int)CSS_Options.IncludeBootstrapAndFonts, Value = "Include Bootstrap and Fonts" },
                new ComboData { Id = (int)CSS_Options.IncludeAOS, Value = "Include AOS" },
                new ComboData { Id = (int)CSS_Options.IncludeAOSHttpsFonts, Value = "Include AOS and Https Fonts" },
                new ComboData { Id = (int)CSS_Options.IncludeAOSBootstrap, Value = "Include AOS and Bootstrap" },
                new ComboData { Id = (int)CSS_Options.IncludeAOSBootstrapAndFonts, Value = "Include AOS, Bootstrap and Fonts" },
                new ComboData { Id = (int)CSS_Options.None, Value = "None" }
            };

            CSSOptions.ItemsSource = ListData;
            CSSOptions.DisplayMemberPath = "Value";
            CSSOptions.SelectedValuePath = "Id";

            // initialize Host Options
            ListData = new List<ComboData>
            {
                new ComboData { Id = (int)Host_Options.ModifyHost, Value = "Modified Host html" },
                new ComboData { Id = (int)Host_Options.PreviewOnly, Value = "Preview Only" },
                new ComboData { Id = (int)Host_Options.None, Value = "None" }
            };

            HostOptions.ItemsSource = ListData;
            HostOptions.DisplayMemberPath = "Value";
            HostOptions.SelectedValuePath = "Id";

            // initialize Script Options
            ListData = new List<ComboData>
            {
                new ComboData { Id = (int)Script_Options.IgnoreScript, Value = "Ignore Scripts" },
                new ComboData { Id = (int)Script_Options.StripMultiLine, Value = "Strip Multi-Line Scripts" },
                new ComboData { Id = (int)Script_Options.SelectedScriptToHost, Value =  "Selected Scripts to Host" },
                new ComboData { Id = (int)Script_Options.All, Value = "All" },
                new ComboData { Id = (int)Script_Options.None, Value = "None" }
            };

            ScriptOptions.ItemsSource = ListData;
            ScriptOptions.DisplayMemberPath = "Value";
            ScriptOptions.SelectedValuePath = "Id";

            // initialize NavMenu Options
            ListData = new List<ComboData>
            {
                new ComboData { Id = (int)NavMenu_Options.AddMenuItem, Value = "Add to NavMenu" },
                new ComboData { Id = (int)NavMenu_Options.PreviewOnly, Value = "Preview Only" },
                new ComboData { Id = (int)NavMenu_Options.None, Value = "None" }
            };

            NavMenuOptions.ItemsSource = ListData;
            NavMenuOptions.DisplayMemberPath = "Value";
            NavMenuOptions.SelectedValuePath = "Id";

            // initialize Razor Options
            ListData = new List<ComboData>
            {
                new ComboData { Id = (int)Page_Options.ConvertBodyStyleBackground, Value = "Convert Body Style Background" },
                new ComboData { Id = (int)Page_Options.None, Value = "None" }
            };

            PageOptions.ItemsSource = ListData;
            PageOptions.DisplayMemberPath = "Value";
            PageOptions.SelectedValuePath = "Id";

            // initialize RenderMode Options
            ListData = new List<ComboData>
            {
                new ComboData { Id = (int)RenderMode_Options.ServerPrerendered, Value = "Server Prerendered" },
                new ComboData { Id = (int)RenderMode_Options.Server, Value = "Server" },
                new ComboData { Id = (int)RenderMode_Options.Static, Value = "Static" },
                new ComboData { Id = (int)RenderMode_Options.NoChange, Value = "No Change or Not Appliable" }
            };

            RenderModeOptions.ItemsSource = ListData;
            RenderModeOptions.DisplayMemberPath = "Value";
            RenderModeOptions.SelectedValuePath = "Id";

            ResetOptions();
            UpdateProjectType();
        }

        private void ResetOptions()
        {
            ModifyOptions.SelectedValue = (int)Modify_Options.Default;
            AssetOptions.SelectedValue = (int)Asset_Options.Default;
            CSSOptions.SelectedValue = (int)CSS_Options.Default;
            HostOptions.SelectedValue = (int)Host_Options.Default;
            ScriptOptions.SelectedValue = (int)Script_Options.Default;
            NavMenuOptions.SelectedValue = (int)NavMenu_Options.Default;
            PageOptions.SelectedValue = (int)Page_Options.Default;
            RenderModeOptions.SelectedValue = (int)RenderMode_Options.Default;
        }

        private bool SetOptions()
        {
            bool result = false;
            if (CheckCSSIsolation() == true)
            {
                CSSOptions.SelectedValue = (int)CSS_Options.IncludeAOSHttpsFonts;
                AssetOptions.SelectedValue = (int)Asset_Options.SharedPagesMove;
                RenderModeOptions.SelectedValue = (int)RenderMode_Options.NoChange;
                result = true;
            }
            else
            {
                CSSOptions.SelectedValue = (int)CSS_Options.Default;
                AssetOptions.SelectedValue = (int)Asset_Options.Default;
                RenderModeOptions.SelectedValue = (int)RenderMode_Options.Default;
            }
            return result;
        }

        private void UpdateOptionsData()
        {
            CurrentOptions.PageName = Page.Text;
            if (CurrentOptions.AllowHtmlEditing == true)
            {
                CurrentOptions.BootstrapIndexHtml = IndexPage.Text;
                CurrentOptions.BootStrapOverrideHtml = Html.Text + CurrentOptions.BootstrapIndexHtml;
            }
            CurrentOptions.SelectedAssetOptions = (Asset_Options)AssetOptions.SelectedValue;
            CurrentOptions.SelectedCSSOptions = (CSS_Options)CSSOptions.SelectedValue;
            CurrentOptions.SelectedHostOptions = (Host_Options)HostOptions.SelectedValue;
            CurrentOptions.SelectedModifyOptions = (Modify_Options)ModifyOptions.SelectedValue;
            CurrentOptions.SelectedNavMenuOptions = (NavMenu_Options)NavMenuOptions.SelectedValue;
            CurrentOptions.SelectedRazorOptions = (Page_Options)PageOptions.SelectedValue;
            CurrentOptions.SelectedRenderModeOptions = (RenderMode_Options)RenderModeOptions.SelectedValue;
            CurrentOptions.SelectedScriptOptions = (Script_Options)ScriptOptions.SelectedValue;
        }

        private void UpdateProjectType()
        {
            string str = Options.GetProjectTypeString(CurrentProjectType);

            ContentType.Text = str;

            if (CurrentOptions.IsReady() == false)
            {
                Create.IsEnabled = false;
                Preview.IsEnabled = false;
                Edit.IsEnabled = false;
                Details.IsEnabled = false;
            }
            else
            {
                Create.IsEnabled = true;
                Preview.IsEnabled = true;
                Edit.IsEnabled = true;
                Details.IsEnabled = true;
            }

            if (CurrentProjectType == Type_Options.BlazorServer)
                RenderModeOptions.SelectedValue = (int)RenderMode_Options.Default;
            else
                RenderModeOptions.SelectedValue = (int)RenderMode_Options.NoChange;
        }

        private void UpdateScreen()
        {
            Contents.Text = CurrentOptions.BlazorProjectPath;
            Html.Text = CurrentOptions.BootstrapProjectPath;
            IndexPage.Text = CurrentOptions.BootstrapIndexHtml;
            Page.Text = CurrentOptions.PageName;
            UpdateProjectType();
        }

        #endregion Screen depended support

        #region Misc support

        private bool CheckCSSIsolation()
        {
            bool result = true;
            CSS_Options cssOptions = (CSS_Options)CSSOptions.SelectedValue;
            Asset_Options assetOptions = (Asset_Options)AssetOptions.SelectedValue;

            if ((DataInfo.IsWebAssemblyType(CurrentProjectType) == true) &&
                 (Options.IsCSSWebassemblyAllowed(cssOptions, assetOptions) == false))
            {
                if (MessageBox.Show("Do wish continue? CSSIsolation not supported", MsgTitle, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    result = false;
                }
            }
            return result;
        }

        private DataInfo GenerateData(bool previewOnly, bool allowOutput)
        {
            UpdateOptionsData();
            DataInfo info = DlgSupport.GenerateData(CurrentOptions, previewOnly, allowOutput);
            CurrentProjectType = info.ProjectType;
            info.IsBootstrapHtml = IsBootstrapHtml;

            return info;
        }

        #endregion Misc support
    }
}