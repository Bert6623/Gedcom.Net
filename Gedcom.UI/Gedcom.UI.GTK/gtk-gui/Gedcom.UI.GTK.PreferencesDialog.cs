// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Gedcom.UI.GTK {
    
    
    public partial class PreferencesDialog {
        
        private Gtk.Notebook notebook1;
        
        private Gtk.VBox vbox2;
        
        private Gtk.CheckButton LoadLastOpenedCheckbutton;
        
        private Gtk.Label label1;
        
        private Gtk.Table table1;
        
        private Gedcom.UI.GTK.Widgets.AddressView AddressView;
        
        private Gtk.Label label4;
        
        private Gtk.Entry NameEntry;
        
        private Gtk.Label label2;
        
        private Gtk.VBox vbox3;
        
        private Gtk.Frame frame1;
        
        private Gtk.Alignment GtkAlignment1;
        
        private Gtk.Table table2;
        
        private Gtk.CheckButton AllowHypenAndUnderscoreLoadCheckbutton;
        
        private Gtk.CheckButton AllowInformationSeparatorOneLoadCheckbutton;
        
        private Gtk.CheckButton AllowInformationSeparatorOneSaveCheckbutton;
        
        private Gtk.CheckButton AllowLineTabsLoadCheckbutton;
        
        private Gtk.CheckButton AllowLineTabsSaveCheckbutton;
        
        private Gtk.CheckButton AllowTabsLoadCheckbutton;
        
        private Gtk.CheckButton AllowTabsSaveCheckbutton;
        
        private Gtk.CheckButton ApplyConcContOnNewLineHackCheckbutton;
        
        private Gtk.CheckButton IgnoreInvalidDelimeterCheckbutton;
        
        private Gtk.CheckButton IgnoreMissingLineTerminatorCheckbutton;
        
        private Gtk.Label label10;
        
        private Gtk.Label label11;
        
        private Gtk.Label label12;
        
        private Gtk.Label label13;
        
        private Gtk.Label label5;
        
        private Gtk.Label label6;
        
        private Gtk.Label label7;
        
        private Gtk.Label label8;
        
        private Gtk.Label label9;
        
        private Gtk.Label GtkLabel11;
        
        private Gtk.Label label3;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget Gedcom.UI.GTK.PreferencesDialog
            this.Name = "Gedcom.UI.GTK.PreferencesDialog";
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.HasSeparator = false;
            // Internal child Gedcom.UI.GTK.PreferencesDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.notebook1 = new Gtk.Notebook();
            this.notebook1.CanFocus = true;
            this.notebook1.Name = "notebook1";
            this.notebook1.CurrentPage = 2;
            this.notebook1.BorderWidth = ((uint)(6));
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            this.vbox2.BorderWidth = ((uint)(6));
            // Container child vbox2.Gtk.Box+BoxChild
            this.LoadLastOpenedCheckbutton = new Gtk.CheckButton();
            this.LoadLastOpenedCheckbutton.CanFocus = true;
            this.LoadLastOpenedCheckbutton.Name = "LoadLastOpenedCheckbutton";
            this.LoadLastOpenedCheckbutton.Label = Mono.Unix.Catalog.GetString("Load Last Opened File on Startup");
            this.LoadLastOpenedCheckbutton.DrawIndicator = true;
            this.LoadLastOpenedCheckbutton.UseUnderline = true;
            this.vbox2.Add(this.LoadLastOpenedCheckbutton);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox2[this.LoadLastOpenedCheckbutton]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            this.notebook1.Add(this.vbox2);
            // Notebook tab
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("General");
            this.notebook1.SetTabLabel(this.vbox2, this.label1);
            this.label1.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.table1 = new Gtk.Table(((uint)(2)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            this.table1.BorderWidth = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.AddressView = new Gedcom.UI.GTK.Widgets.AddressView();
            this.AddressView.Events = ((Gdk.EventMask)(256));
            this.AddressView.Name = "AddressView";
            this.table1.Add(this.AddressView);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table1[this.AddressView]));
            w4.TopAttach = ((uint)(1));
            w4.BottomAttach = ((uint)(2));
            w4.RightAttach = ((uint)(2));
            // Container child table1.Gtk.Table+TableChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.Xalign = 0F;
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Name:");
            this.table1.Add(this.label4);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table1[this.label4]));
            w5.XOptions = ((Gtk.AttachOptions)(4));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.NameEntry = new Gtk.Entry();
            this.NameEntry.CanFocus = true;
            this.NameEntry.Name = "NameEntry";
            this.NameEntry.IsEditable = true;
            this.NameEntry.InvisibleChar = '●';
            this.table1.Add(this.NameEntry);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table1[this.NameEntry]));
            w6.LeftAttach = ((uint)(1));
            w6.RightAttach = ((uint)(2));
            w6.YOptions = ((Gtk.AttachOptions)(4));
            this.notebook1.Add(this.table1);
            Gtk.Notebook.NotebookChild w7 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.table1]));
            w7.Position = 1;
            // Notebook tab
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Submitter");
            this.notebook1.SetTabLabel(this.table1, this.label2);
            this.label2.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            this.vbox3.BorderWidth = ((uint)(6));
            // Container child vbox3.Gtk.Box+BoxChild
            this.frame1 = new Gtk.Frame();
            this.frame1.Name = "frame1";
            this.frame1.ShadowType = ((Gtk.ShadowType)(0));
            // Container child frame1.Gtk.Container+ContainerChild
            this.GtkAlignment1 = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.GtkAlignment1.Name = "GtkAlignment1";
            this.GtkAlignment1.LeftPadding = ((uint)(12));
            // Container child GtkAlignment1.Gtk.Container+ContainerChild
            this.table2 = new Gtk.Table(((uint)(8)), ((uint)(3)), false);
            this.table2.Name = "table2";
            this.table2.RowSpacing = ((uint)(6));
            this.table2.ColumnSpacing = ((uint)(6));
            this.table2.BorderWidth = ((uint)(6));
            // Container child table2.Gtk.Table+TableChild
            this.AllowHypenAndUnderscoreLoadCheckbutton = new Gtk.CheckButton();
            this.AllowHypenAndUnderscoreLoadCheckbutton.CanFocus = true;
            this.AllowHypenAndUnderscoreLoadCheckbutton.Name = "AllowHypenAndUnderscoreLoadCheckbutton";
            this.AllowHypenAndUnderscoreLoadCheckbutton.Label = "";
            this.AllowHypenAndUnderscoreLoadCheckbutton.DrawIndicator = true;
            this.AllowHypenAndUnderscoreLoadCheckbutton.UseUnderline = true;
            this.table2.Add(this.AllowHypenAndUnderscoreLoadCheckbutton);
            Gtk.Table.TableChild w8 = ((Gtk.Table.TableChild)(this.table2[this.AllowHypenAndUnderscoreLoadCheckbutton]));
            w8.TopAttach = ((uint)(1));
            w8.BottomAttach = ((uint)(2));
            w8.LeftAttach = ((uint)(1));
            w8.RightAttach = ((uint)(2));
            w8.XOptions = ((Gtk.AttachOptions)(4));
            w8.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.AllowInformationSeparatorOneLoadCheckbutton = new Gtk.CheckButton();
            this.AllowInformationSeparatorOneLoadCheckbutton.CanFocus = true;
            this.AllowInformationSeparatorOneLoadCheckbutton.Name = "AllowInformationSeparatorOneLoadCheckbutton";
            this.AllowInformationSeparatorOneLoadCheckbutton.Label = "";
            this.AllowInformationSeparatorOneLoadCheckbutton.DrawIndicator = true;
            this.AllowInformationSeparatorOneLoadCheckbutton.UseUnderline = true;
            this.table2.Add(this.AllowInformationSeparatorOneLoadCheckbutton);
            Gtk.Table.TableChild w9 = ((Gtk.Table.TableChild)(this.table2[this.AllowInformationSeparatorOneLoadCheckbutton]));
            w9.TopAttach = ((uint)(2));
            w9.BottomAttach = ((uint)(3));
            w9.LeftAttach = ((uint)(1));
            w9.RightAttach = ((uint)(2));
            w9.XOptions = ((Gtk.AttachOptions)(4));
            w9.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.AllowInformationSeparatorOneSaveCheckbutton = new Gtk.CheckButton();
            this.AllowInformationSeparatorOneSaveCheckbutton.CanFocus = true;
            this.AllowInformationSeparatorOneSaveCheckbutton.Name = "AllowInformationSeparatorOneSaveCheckbutton";
            this.AllowInformationSeparatorOneSaveCheckbutton.Label = "";
            this.AllowInformationSeparatorOneSaveCheckbutton.DrawIndicator = true;
            this.AllowInformationSeparatorOneSaveCheckbutton.UseUnderline = true;
            this.table2.Add(this.AllowInformationSeparatorOneSaveCheckbutton);
            Gtk.Table.TableChild w10 = ((Gtk.Table.TableChild)(this.table2[this.AllowInformationSeparatorOneSaveCheckbutton]));
            w10.TopAttach = ((uint)(2));
            w10.BottomAttach = ((uint)(3));
            w10.LeftAttach = ((uint)(2));
            w10.RightAttach = ((uint)(3));
            w10.XOptions = ((Gtk.AttachOptions)(4));
            w10.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.AllowLineTabsLoadCheckbutton = new Gtk.CheckButton();
            this.AllowLineTabsLoadCheckbutton.CanFocus = true;
            this.AllowLineTabsLoadCheckbutton.Name = "AllowLineTabsLoadCheckbutton";
            this.AllowLineTabsLoadCheckbutton.Label = "";
            this.AllowLineTabsLoadCheckbutton.DrawIndicator = true;
            this.AllowLineTabsLoadCheckbutton.UseUnderline = true;
            this.table2.Add(this.AllowLineTabsLoadCheckbutton);
            Gtk.Table.TableChild w11 = ((Gtk.Table.TableChild)(this.table2[this.AllowLineTabsLoadCheckbutton]));
            w11.TopAttach = ((uint)(3));
            w11.BottomAttach = ((uint)(4));
            w11.LeftAttach = ((uint)(1));
            w11.RightAttach = ((uint)(2));
            w11.XOptions = ((Gtk.AttachOptions)(4));
            w11.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.AllowLineTabsSaveCheckbutton = new Gtk.CheckButton();
            this.AllowLineTabsSaveCheckbutton.CanFocus = true;
            this.AllowLineTabsSaveCheckbutton.Name = "AllowLineTabsSaveCheckbutton";
            this.AllowLineTabsSaveCheckbutton.Label = "";
            this.AllowLineTabsSaveCheckbutton.DrawIndicator = true;
            this.AllowLineTabsSaveCheckbutton.UseUnderline = true;
            this.table2.Add(this.AllowLineTabsSaveCheckbutton);
            Gtk.Table.TableChild w12 = ((Gtk.Table.TableChild)(this.table2[this.AllowLineTabsSaveCheckbutton]));
            w12.TopAttach = ((uint)(3));
            w12.BottomAttach = ((uint)(4));
            w12.LeftAttach = ((uint)(2));
            w12.RightAttach = ((uint)(3));
            w12.XOptions = ((Gtk.AttachOptions)(4));
            w12.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.AllowTabsLoadCheckbutton = new Gtk.CheckButton();
            this.AllowTabsLoadCheckbutton.CanFocus = true;
            this.AllowTabsLoadCheckbutton.Name = "AllowTabsLoadCheckbutton";
            this.AllowTabsLoadCheckbutton.Label = "";
            this.AllowTabsLoadCheckbutton.DrawIndicator = true;
            this.AllowTabsLoadCheckbutton.UseUnderline = true;
            this.table2.Add(this.AllowTabsLoadCheckbutton);
            Gtk.Table.TableChild w13 = ((Gtk.Table.TableChild)(this.table2[this.AllowTabsLoadCheckbutton]));
            w13.TopAttach = ((uint)(4));
            w13.BottomAttach = ((uint)(5));
            w13.LeftAttach = ((uint)(1));
            w13.RightAttach = ((uint)(2));
            w13.XOptions = ((Gtk.AttachOptions)(4));
            w13.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.AllowTabsSaveCheckbutton = new Gtk.CheckButton();
            this.AllowTabsSaveCheckbutton.CanFocus = true;
            this.AllowTabsSaveCheckbutton.Name = "AllowTabsSaveCheckbutton";
            this.AllowTabsSaveCheckbutton.Label = "";
            this.AllowTabsSaveCheckbutton.DrawIndicator = true;
            this.AllowTabsSaveCheckbutton.UseUnderline = true;
            this.table2.Add(this.AllowTabsSaveCheckbutton);
            Gtk.Table.TableChild w14 = ((Gtk.Table.TableChild)(this.table2[this.AllowTabsSaveCheckbutton]));
            w14.TopAttach = ((uint)(4));
            w14.BottomAttach = ((uint)(5));
            w14.LeftAttach = ((uint)(2));
            w14.RightAttach = ((uint)(3));
            w14.XOptions = ((Gtk.AttachOptions)(4));
            w14.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.ApplyConcContOnNewLineHackCheckbutton = new Gtk.CheckButton();
            this.ApplyConcContOnNewLineHackCheckbutton.CanFocus = true;
            this.ApplyConcContOnNewLineHackCheckbutton.Name = "ApplyConcContOnNewLineHackCheckbutton";
            this.ApplyConcContOnNewLineHackCheckbutton.Label = "";
            this.ApplyConcContOnNewLineHackCheckbutton.DrawIndicator = true;
            this.ApplyConcContOnNewLineHackCheckbutton.UseUnderline = true;
            this.table2.Add(this.ApplyConcContOnNewLineHackCheckbutton);
            Gtk.Table.TableChild w15 = ((Gtk.Table.TableChild)(this.table2[this.ApplyConcContOnNewLineHackCheckbutton]));
            w15.TopAttach = ((uint)(5));
            w15.BottomAttach = ((uint)(6));
            w15.LeftAttach = ((uint)(1));
            w15.RightAttach = ((uint)(2));
            w15.XOptions = ((Gtk.AttachOptions)(4));
            w15.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.IgnoreInvalidDelimeterCheckbutton = new Gtk.CheckButton();
            this.IgnoreInvalidDelimeterCheckbutton.CanFocus = true;
            this.IgnoreInvalidDelimeterCheckbutton.Name = "IgnoreInvalidDelimeterCheckbutton";
            this.IgnoreInvalidDelimeterCheckbutton.Label = "";
            this.IgnoreInvalidDelimeterCheckbutton.DrawIndicator = true;
            this.IgnoreInvalidDelimeterCheckbutton.UseUnderline = true;
            this.table2.Add(this.IgnoreInvalidDelimeterCheckbutton);
            Gtk.Table.TableChild w16 = ((Gtk.Table.TableChild)(this.table2[this.IgnoreInvalidDelimeterCheckbutton]));
            w16.TopAttach = ((uint)(6));
            w16.BottomAttach = ((uint)(7));
            w16.LeftAttach = ((uint)(1));
            w16.RightAttach = ((uint)(2));
            w16.XOptions = ((Gtk.AttachOptions)(4));
            w16.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.IgnoreMissingLineTerminatorCheckbutton = new Gtk.CheckButton();
            this.IgnoreMissingLineTerminatorCheckbutton.CanFocus = true;
            this.IgnoreMissingLineTerminatorCheckbutton.Name = "IgnoreMissingLineTerminatorCheckbutton";
            this.IgnoreMissingLineTerminatorCheckbutton.Label = "";
            this.IgnoreMissingLineTerminatorCheckbutton.DrawIndicator = true;
            this.IgnoreMissingLineTerminatorCheckbutton.UseUnderline = true;
            this.table2.Add(this.IgnoreMissingLineTerminatorCheckbutton);
            Gtk.Table.TableChild w17 = ((Gtk.Table.TableChild)(this.table2[this.IgnoreMissingLineTerminatorCheckbutton]));
            w17.TopAttach = ((uint)(7));
            w17.BottomAttach = ((uint)(8));
            w17.LeftAttach = ((uint)(1));
            w17.RightAttach = ((uint)(2));
            w17.XOptions = ((Gtk.AttachOptions)(4));
            w17.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label10 = new Gtk.Label();
            this.label10.Name = "label10";
            this.label10.Xalign = 0F;
            this.label10.LabelProp = Mono.Unix.Catalog.GetString("Apply Conc / Cont On New Line Hack");
            this.table2.Add(this.label10);
            Gtk.Table.TableChild w18 = ((Gtk.Table.TableChild)(this.table2[this.label10]));
            w18.TopAttach = ((uint)(5));
            w18.BottomAttach = ((uint)(6));
            w18.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label11 = new Gtk.Label();
            this.label11.Name = "label11";
            this.label11.Xalign = 0F;
            this.label11.LabelProp = Mono.Unix.Catalog.GetString("Ignore Invalid Delimeter");
            this.table2.Add(this.label11);
            Gtk.Table.TableChild w19 = ((Gtk.Table.TableChild)(this.table2[this.label11]));
            w19.TopAttach = ((uint)(6));
            w19.BottomAttach = ((uint)(7));
            w19.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label12 = new Gtk.Label();
            this.label12.Name = "label12";
            this.label12.Xalign = 0F;
            this.label12.LabelProp = Mono.Unix.Catalog.GetString("Ignore Missing Line Terminator");
            this.table2.Add(this.label12);
            Gtk.Table.TableChild w20 = ((Gtk.Table.TableChild)(this.table2[this.label12]));
            w20.TopAttach = ((uint)(7));
            w20.BottomAttach = ((uint)(8));
            w20.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label13 = new Gtk.Label();
            this.label13.Name = "label13";
            this.label13.Xalign = 0F;
            this.label13.LabelProp = Mono.Unix.Catalog.GetString("Allow Line Tabs");
            this.table2.Add(this.label13);
            Gtk.Table.TableChild w21 = ((Gtk.Table.TableChild)(this.table2[this.label13]));
            w21.TopAttach = ((uint)(3));
            w21.BottomAttach = ((uint)(4));
            w21.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("<b>Load</b>");
            this.label5.UseMarkup = true;
            this.table2.Add(this.label5);
            Gtk.Table.TableChild w22 = ((Gtk.Table.TableChild)(this.table2[this.label5]));
            w22.LeftAttach = ((uint)(1));
            w22.RightAttach = ((uint)(2));
            w22.XOptions = ((Gtk.AttachOptions)(4));
            w22.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("<b>Save</b>");
            this.label6.UseMarkup = true;
            this.table2.Add(this.label6);
            Gtk.Table.TableChild w23 = ((Gtk.Table.TableChild)(this.table2[this.label6]));
            w23.LeftAttach = ((uint)(2));
            w23.RightAttach = ((uint)(3));
            w23.XOptions = ((Gtk.AttachOptions)(4));
            w23.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label7 = new Gtk.Label();
            this.label7.Name = "label7";
            this.label7.Xalign = 0F;
            this.label7.LabelProp = Mono.Unix.Catalog.GetString("Allow \"-\" and \"_\" in tag names");
            this.table2.Add(this.label7);
            Gtk.Table.TableChild w24 = ((Gtk.Table.TableChild)(this.table2[this.label7]));
            w24.TopAttach = ((uint)(1));
            w24.BottomAttach = ((uint)(2));
            w24.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.Xalign = 0F;
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("Allow Information Separator One Character");
            this.table2.Add(this.label8);
            Gtk.Table.TableChild w25 = ((Gtk.Table.TableChild)(this.table2[this.label8]));
            w25.TopAttach = ((uint)(2));
            w25.BottomAttach = ((uint)(3));
            w25.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label9 = new Gtk.Label();
            this.label9.Name = "label9";
            this.label9.Xalign = 0F;
            this.label9.LabelProp = Mono.Unix.Catalog.GetString("Allow Tabs");
            this.table2.Add(this.label9);
            Gtk.Table.TableChild w26 = ((Gtk.Table.TableChild)(this.table2[this.label9]));
            w26.TopAttach = ((uint)(4));
            w26.BottomAttach = ((uint)(5));
            w26.XOptions = ((Gtk.AttachOptions)(4));
            w26.YOptions = ((Gtk.AttachOptions)(4));
            this.GtkAlignment1.Add(this.table2);
            this.frame1.Add(this.GtkAlignment1);
            this.GtkLabel11 = new Gtk.Label();
            this.GtkLabel11.Name = "GtkLabel11";
            this.GtkLabel11.LabelProp = Mono.Unix.Catalog.GetString("<b>GEDCOM Settings</b>");
            this.GtkLabel11.UseMarkup = true;
            this.frame1.LabelWidget = this.GtkLabel11;
            this.vbox3.Add(this.frame1);
            Gtk.Box.BoxChild w29 = ((Gtk.Box.BoxChild)(this.vbox3[this.frame1]));
            w29.Position = 0;
            w29.Expand = false;
            w29.Fill = false;
            this.notebook1.Add(this.vbox3);
            Gtk.Notebook.NotebookChild w30 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.vbox3]));
            w30.Position = 2;
            // Notebook tab
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("Advanced");
            this.notebook1.SetTabLabel(this.vbox3, this.label3);
            this.label3.ShowAll();
            w1.Add(this.notebook1);
            Gtk.Box.BoxChild w31 = ((Gtk.Box.BoxChild)(w1[this.notebook1]));
            w31.Position = 0;
            // Internal child Gedcom.UI.GTK.PreferencesDialog.ActionArea
            Gtk.HButtonBox w32 = this.ActionArea;
            w32.Name = "dialog1_ActionArea";
            w32.Spacing = 6;
            w32.BorderWidth = ((uint)(5));
            w32.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-close";
            this.AddActionWidget(this.buttonOk, -7);
            Gtk.ButtonBox.ButtonBoxChild w33 = ((Gtk.ButtonBox.ButtonBoxChild)(w32[this.buttonOk]));
            w33.Expand = false;
            w33.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 464;
            this.DefaultHeight = 399;
            this.Show();
            this.LoadLastOpenedCheckbutton.Toggled += new System.EventHandler(this.LoadLastOpenedCheckbutton_Toggled);
            this.IgnoreMissingLineTerminatorCheckbutton.Toggled += new System.EventHandler(this.IgnoreMissingLineTerminatorCheckbutton_Toggled);
            this.IgnoreInvalidDelimeterCheckbutton.Toggled += new System.EventHandler(this.IgnoreInvalidDelimeterCheckbutton_Toggled);
            this.ApplyConcContOnNewLineHackCheckbutton.Toggled += new System.EventHandler(this.ApplyConcContOnNewLineHackCheckbutton_Toggled);
            this.AllowTabsSaveCheckbutton.Toggled += new System.EventHandler(this.AllowTabsSaveCheckbutton_Toggled);
            this.AllowTabsLoadCheckbutton.Toggled += new System.EventHandler(this.AllowTabsLoadCheckbutton_Toggled);
            this.AllowLineTabsSaveCheckbutton.Toggled += new System.EventHandler(this.AllowLineTabsSaveCheckbutton_Toggled);
            this.AllowLineTabsLoadCheckbutton.Toggled += new System.EventHandler(this.AllowLineTabsLoadCheckbutton_Toggled);
            this.AllowInformationSeparatorOneSaveCheckbutton.Toggled += new System.EventHandler(this.AllowInformationSeparatorOneSaveCheckbutton_Toggled);
            this.AllowInformationSeparatorOneLoadCheckbutton.Toggled += new System.EventHandler(this.AllowInformationSeparatorOneLoadCheckbutton_Toggled);
            this.AllowHypenAndUnderscoreLoadCheckbutton.Toggled += new System.EventHandler(this.AllowHypenAndUnderscoreLoadCheckbutton_Toggled);
        }
    }
}
