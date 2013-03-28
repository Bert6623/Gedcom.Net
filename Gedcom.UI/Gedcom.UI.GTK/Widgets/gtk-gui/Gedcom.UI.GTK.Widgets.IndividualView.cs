// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Gedcom.UI.GTK.Widgets {
    
    
    public partial class IndividualView {
        
        private Gtk.VBox vbox1;
        
        private Gtk.Notebook Notebook;
        
        private Gtk.Table table1;
        
        private Gtk.Entry BornInEntry;
        
        private Gtk.Entry DateBornEntry;
        
        private Gtk.Button DateBornSourceButton;
        
        private Gtk.Image image440;
        
        private Gtk.Entry DateDiedEntry;
        
        private Gtk.Button DateDiedSourceButton;
        
        private Gtk.Image image438;
        
        private Gtk.Button DeleteButton;
        
        private Gtk.Image image415;
        
        private Gtk.Entry DiedInEntry;
        
        private Gedcom.UI.GTK.Widgets.FactView FactView;
        
        private Gtk.HSeparator hseparator3;
        
        private Gtk.Button IndiScrapbookButton;
        
        private Gtk.Label label10;
        
        private Gtk.Label label11;
        
        private Gtk.Label label18;
        
        private Gtk.Label label19;
        
        private Gtk.Label label9;
        
        private Gtk.Button NameButton;
        
        private Gtk.Label SpouseNameLabel;
        
        private Gtk.Entry NameEntry;
        
        private Gtk.Button NameSourceButton;
        
        private Gtk.Image image439;
        
        private Gtk.ComboBox SexComboBox;
        
        private Gtk.Label label2;
        
        private Gtk.Table table3;
        
        private Gtk.HSeparator hseparator4;
        
        private Gedcom.UI.GTK.Widgets.MarriageView MarriageView;
        
        private Gtk.ScrolledWindow scrolledwindow8;
        
        private Gtk.TreeView MarriageTreeView;
        
        private Gtk.Label label6;
        
        private Gedcom.UI.GTK.Widgets.AddressView AddressView;
        
        private Gtk.Label label3;
        
        private Gtk.Table table6;
        
        private Gtk.Entry HeightEntry;
        
        private Gtk.Label label30;
        
        private Gtk.Label label33;
        
        private Gtk.Label label34;
        
        private Gtk.Label label35;
        
        private Gtk.ScrolledWindow scrolledwindow5;
        
        private Gtk.TextView MedicalInformationTextView;
        
        private Gtk.ScrolledWindow scrolledwindow6;
        
        private Gtk.TextView CauseOfDeathTextView;
        
        private Gtk.Entry WeightEntry;
        
        private Gtk.Label label4;
        
        private Gtk.Label label5;
        
        private Gtk.Label label40;
        
        private Gedcom.UI.GTK.Widgets.NotesView NotesView;
        
        private Gtk.Label label7;
        
        private Gtk.VBox vbox5;
        
        private Gtk.VBox SwitchBox;
        
        private Gtk.Label label1;
        
        private Gtk.HBox hbox3;
        
        private Gedcom.UI.GTK.Widgets.IndividualListComboBox ParentsCombo;
        
        private Gedcom.UI.GTK.Widgets.IndividualListComboBox ChildrenCombo;
        
        private Gedcom.UI.GTK.Widgets.IndividualListComboBox SpousesCombo;
        
        private Gedcom.UI.GTK.Widgets.IndividualListComboBox SiblingsCombo;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget Gedcom.UI.GTK.Widgets.IndividualView
            Stetic.BinContainer.Attach(this);
            this.Events = ((Gdk.EventMask)(256));
            this.Name = "Gedcom.UI.GTK.Widgets.IndividualView";
            // Container child Gedcom.UI.GTK.Widgets.IndividualView.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            this.vbox1.BorderWidth = ((uint)(6));
            // Container child vbox1.Gtk.Box+BoxChild
            this.Notebook = new Gtk.Notebook();
            this.Notebook.CanFocus = true;
            this.Notebook.Name = "Notebook";
            this.Notebook.CurrentPage = 3;
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.table1 = new Gtk.Table(((uint)(5)), ((uint)(7)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(12));
            this.table1.BorderWidth = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.BornInEntry = new Gtk.Entry();
            this.BornInEntry.CanFocus = true;
            this.BornInEntry.Name = "BornInEntry";
            this.BornInEntry.IsEditable = true;
            this.BornInEntry.InvisibleChar = '●';
            this.table1.Add(this.BornInEntry);
            Gtk.Table.TableChild w1 = ((Gtk.Table.TableChild)(this.table1[this.BornInEntry]));
            w1.TopAttach = ((uint)(1));
            w1.BottomAttach = ((uint)(2));
            w1.LeftAttach = ((uint)(4));
            w1.RightAttach = ((uint)(7));
            w1.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.DateBornEntry = new Gtk.Entry();
            this.DateBornEntry.CanFocus = true;
            this.DateBornEntry.Name = "DateBornEntry";
            this.DateBornEntry.IsEditable = true;
            this.DateBornEntry.InvisibleChar = '●';
            this.table1.Add(this.DateBornEntry);
            Gtk.Table.TableChild w2 = ((Gtk.Table.TableChild)(this.table1[this.DateBornEntry]));
            w2.TopAttach = ((uint)(1));
            w2.BottomAttach = ((uint)(2));
            w2.LeftAttach = ((uint)(1));
            w2.RightAttach = ((uint)(2));
            w2.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.DateBornSourceButton = new Gtk.Button();
            this.DateBornSourceButton.CanFocus = true;
            this.DateBornSourceButton.Name = "DateBornSourceButton";
            // Container child DateBornSourceButton.Gtk.Container+ContainerChild
            this.image440 = new Gtk.Image();
            this.image440.Name = "image440";
            this.image440.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-index", Gtk.IconSize.Button, 20);
            this.DateBornSourceButton.Add(this.image440);
            this.DateBornSourceButton.Label = null;
            this.table1.Add(this.DateBornSourceButton);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table1[this.DateBornSourceButton]));
            w4.TopAttach = ((uint)(1));
            w4.BottomAttach = ((uint)(2));
            w4.LeftAttach = ((uint)(2));
            w4.RightAttach = ((uint)(3));
            w4.XOptions = ((Gtk.AttachOptions)(4));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.DateDiedEntry = new Gtk.Entry();
            this.DateDiedEntry.CanFocus = true;
            this.DateDiedEntry.Name = "DateDiedEntry";
            this.DateDiedEntry.IsEditable = true;
            this.DateDiedEntry.InvisibleChar = '●';
            this.table1.Add(this.DateDiedEntry);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table1[this.DateDiedEntry]));
            w5.TopAttach = ((uint)(2));
            w5.BottomAttach = ((uint)(3));
            w5.LeftAttach = ((uint)(1));
            w5.RightAttach = ((uint)(2));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.DateDiedSourceButton = new Gtk.Button();
            this.DateDiedSourceButton.CanFocus = true;
            this.DateDiedSourceButton.Name = "DateDiedSourceButton";
            // Container child DateDiedSourceButton.Gtk.Container+ContainerChild
            this.image438 = new Gtk.Image();
            this.image438.Name = "image438";
            this.image438.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-index", Gtk.IconSize.Button, 20);
            this.DateDiedSourceButton.Add(this.image438);
            this.DateDiedSourceButton.Label = null;
            this.table1.Add(this.DateDiedSourceButton);
            Gtk.Table.TableChild w7 = ((Gtk.Table.TableChild)(this.table1[this.DateDiedSourceButton]));
            w7.TopAttach = ((uint)(2));
            w7.BottomAttach = ((uint)(3));
            w7.LeftAttach = ((uint)(2));
            w7.RightAttach = ((uint)(3));
            w7.XOptions = ((Gtk.AttachOptions)(4));
            w7.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.DeleteButton = new Gtk.Button();
            this.DeleteButton.CanFocus = true;
            this.DeleteButton.Name = "DeleteButton";
            // Container child DeleteButton.Gtk.Container+ContainerChild
            this.image415 = new Gtk.Image();
            this.image415.Name = "image415";
            this.image415.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-delete", Gtk.IconSize.Menu, 16);
            this.DeleteButton.Add(this.image415);
            this.DeleteButton.Label = null;
            this.table1.Add(this.DeleteButton);
            Gtk.Table.TableChild w9 = ((Gtk.Table.TableChild)(this.table1[this.DeleteButton]));
            w9.LeftAttach = ((uint)(6));
            w9.RightAttach = ((uint)(7));
            w9.XOptions = ((Gtk.AttachOptions)(4));
            w9.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.DiedInEntry = new Gtk.Entry();
            this.DiedInEntry.CanFocus = true;
            this.DiedInEntry.Name = "DiedInEntry";
            this.DiedInEntry.IsEditable = true;
            this.DiedInEntry.InvisibleChar = '●';
            this.table1.Add(this.DiedInEntry);
            Gtk.Table.TableChild w10 = ((Gtk.Table.TableChild)(this.table1[this.DiedInEntry]));
            w10.TopAttach = ((uint)(2));
            w10.BottomAttach = ((uint)(3));
            w10.LeftAttach = ((uint)(4));
            w10.RightAttach = ((uint)(7));
            w10.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.FactView = new Gedcom.UI.GTK.Widgets.FactView();
            this.FactView.Events = ((Gdk.EventMask)(256));
            this.FactView.Name = "FactView";
            this.table1.Add(this.FactView);
            Gtk.Table.TableChild w11 = ((Gtk.Table.TableChild)(this.table1[this.FactView]));
            w11.TopAttach = ((uint)(4));
            w11.BottomAttach = ((uint)(5));
            w11.RightAttach = ((uint)(7));
            w11.YOptions = ((Gtk.AttachOptions)(7));
            // Container child table1.Gtk.Table+TableChild
            this.hseparator3 = new Gtk.HSeparator();
            this.hseparator3.Name = "hseparator3";
            this.table1.Add(this.hseparator3);
            Gtk.Table.TableChild w12 = ((Gtk.Table.TableChild)(this.table1[this.hseparator3]));
            w12.TopAttach = ((uint)(3));
            w12.BottomAttach = ((uint)(4));
            w12.RightAttach = ((uint)(7));
            w12.XOptions = ((Gtk.AttachOptions)(4));
            w12.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.IndiScrapbookButton = new Gtk.Button();
            this.IndiScrapbookButton.CanFocus = true;
            this.IndiScrapbookButton.Name = "IndiScrapbookButton";
            // Container child IndiScrapbookButton.Gtk.Container+ContainerChild
            Gtk.Alignment w13 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w14 = new Gtk.HBox();
            w14.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w15 = new Gtk.Image();
            w15.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-paste", Gtk.IconSize.Button, 20);
            w14.Add(w15);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w17 = new Gtk.Label();
            w17.LabelProp = "Scrapbook";
            w14.Add(w17);
            w13.Add(w14);
            this.IndiScrapbookButton.Add(w13);
            this.table1.Add(this.IndiScrapbookButton);
            Gtk.Table.TableChild w21 = ((Gtk.Table.TableChild)(this.table1[this.IndiScrapbookButton]));
            w21.LeftAttach = ((uint)(5));
            w21.RightAttach = ((uint)(6));
            w21.XOptions = ((Gtk.AttachOptions)(4));
            w21.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label10 = new Gtk.Label();
            this.label10.Name = "label10";
            this.label10.LabelProp = "in:";
            this.table1.Add(this.label10);
            Gtk.Table.TableChild w22 = ((Gtk.Table.TableChild)(this.table1[this.label10]));
            w22.TopAttach = ((uint)(1));
            w22.BottomAttach = ((uint)(2));
            w22.LeftAttach = ((uint)(3));
            w22.RightAttach = ((uint)(4));
            w22.XOptions = ((Gtk.AttachOptions)(4));
            w22.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label11 = new Gtk.Label();
            this.label11.Name = "label11";
            this.label11.LabelProp = "in:";
            this.table1.Add(this.label11);
            Gtk.Table.TableChild w23 = ((Gtk.Table.TableChild)(this.table1[this.label11]));
            w23.TopAttach = ((uint)(2));
            w23.BottomAttach = ((uint)(3));
            w23.LeftAttach = ((uint)(3));
            w23.RightAttach = ((uint)(4));
            w23.XOptions = ((Gtk.AttachOptions)(4));
            w23.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label18 = new Gtk.Label();
            this.label18.Name = "label18";
            this.label18.Xalign = 0F;
            this.label18.LabelProp = "Date Born:";
            this.table1.Add(this.label18);
            Gtk.Table.TableChild w24 = ((Gtk.Table.TableChild)(this.table1[this.label18]));
            w24.TopAttach = ((uint)(1));
            w24.BottomAttach = ((uint)(2));
            w24.XOptions = ((Gtk.AttachOptions)(4));
            w24.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label19 = new Gtk.Label();
            this.label19.Name = "label19";
            this.label19.Xalign = 0F;
            this.label19.LabelProp = "Date Died:";
            this.table1.Add(this.label19);
            Gtk.Table.TableChild w25 = ((Gtk.Table.TableChild)(this.table1[this.label19]));
            w25.TopAttach = ((uint)(2));
            w25.BottomAttach = ((uint)(3));
            w25.XOptions = ((Gtk.AttachOptions)(4));
            w25.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label9 = new Gtk.Label();
            this.label9.Name = "label9";
            this.label9.LabelProp = "Sex:";
            this.table1.Add(this.label9);
            Gtk.Table.TableChild w26 = ((Gtk.Table.TableChild)(this.table1[this.label9]));
            w26.LeftAttach = ((uint)(3));
            w26.RightAttach = ((uint)(4));
            w26.XOptions = ((Gtk.AttachOptions)(4));
            w26.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.NameButton = new Gtk.Button();
            this.NameButton.CanFocus = true;
            this.NameButton.Name = "NameButton";
            // Container child NameButton.Gtk.Container+ContainerChild
            this.SpouseNameLabel = new Gtk.Label();
            this.SpouseNameLabel.Name = "SpouseNameLabel";
            this.SpouseNameLabel.Xalign = 1F;
            this.SpouseNameLabel.LabelProp = "Name:";
            this.NameButton.Add(this.SpouseNameLabel);
            this.NameButton.Label = null;
            this.table1.Add(this.NameButton);
            Gtk.Table.TableChild w28 = ((Gtk.Table.TableChild)(this.table1[this.NameButton]));
            w28.XOptions = ((Gtk.AttachOptions)(4));
            w28.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.NameEntry = new Gtk.Entry();
            this.NameEntry.CanFocus = true;
            this.NameEntry.Name = "NameEntry";
            this.NameEntry.IsEditable = true;
            this.NameEntry.InvisibleChar = '●';
            this.table1.Add(this.NameEntry);
            Gtk.Table.TableChild w29 = ((Gtk.Table.TableChild)(this.table1[this.NameEntry]));
            w29.LeftAttach = ((uint)(1));
            w29.RightAttach = ((uint)(2));
            w29.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.NameSourceButton = new Gtk.Button();
            this.NameSourceButton.CanFocus = true;
            this.NameSourceButton.Name = "NameSourceButton";
            // Container child NameSourceButton.Gtk.Container+ContainerChild
            this.image439 = new Gtk.Image();
            this.image439.Name = "image439";
            this.image439.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-index", Gtk.IconSize.Button, 20);
            this.NameSourceButton.Add(this.image439);
            this.NameSourceButton.Label = null;
            this.table1.Add(this.NameSourceButton);
            Gtk.Table.TableChild w31 = ((Gtk.Table.TableChild)(this.table1[this.NameSourceButton]));
            w31.LeftAttach = ((uint)(2));
            w31.RightAttach = ((uint)(3));
            w31.XOptions = ((Gtk.AttachOptions)(4));
            w31.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.SexComboBox = Gtk.ComboBox.NewText();
            this.SexComboBox.AppendText("Undetermined");
            this.SexComboBox.AppendText("Male");
            this.SexComboBox.AppendText("Female");
            this.SexComboBox.AppendText("Both");
            this.SexComboBox.AppendText("Neuter");
            this.SexComboBox.Name = "SexComboBox";
            this.SexComboBox.Active = 0;
            this.table1.Add(this.SexComboBox);
            Gtk.Table.TableChild w32 = ((Gtk.Table.TableChild)(this.table1[this.SexComboBox]));
            w32.LeftAttach = ((uint)(4));
            w32.RightAttach = ((uint)(5));
            w32.XOptions = ((Gtk.AttachOptions)(4));
            w32.YOptions = ((Gtk.AttachOptions)(4));
            this.Notebook.Add(this.table1);
            // Notebook tab
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = "Facts";
            this.Notebook.SetTabLabel(this.table1, this.label2);
            this.label2.ShowAll();
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.table3 = new Gtk.Table(((uint)(3)), ((uint)(5)), false);
            this.table3.Name = "table3";
            this.table3.RowSpacing = ((uint)(6));
            this.table3.ColumnSpacing = ((uint)(12));
            this.table3.BorderWidth = ((uint)(6));
            // Container child table3.Gtk.Table+TableChild
            this.hseparator4 = new Gtk.HSeparator();
            this.hseparator4.Name = "hseparator4";
            this.table3.Add(this.hseparator4);
            Gtk.Table.TableChild w34 = ((Gtk.Table.TableChild)(this.table3[this.hseparator4]));
            w34.TopAttach = ((uint)(1));
            w34.BottomAttach = ((uint)(2));
            w34.RightAttach = ((uint)(5));
            w34.XOptions = ((Gtk.AttachOptions)(4));
            w34.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.MarriageView = new Gedcom.UI.GTK.Widgets.MarriageView();
            this.MarriageView.Events = ((Gdk.EventMask)(256));
            this.MarriageView.Name = "MarriageView";
            this.table3.Add(this.MarriageView);
            Gtk.Table.TableChild w35 = ((Gtk.Table.TableChild)(this.table3[this.MarriageView]));
            w35.RightAttach = ((uint)(5));
            w35.XOptions = ((Gtk.AttachOptions)(4));
            w35.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.scrolledwindow8 = new Gtk.ScrolledWindow();
            this.scrolledwindow8.CanFocus = true;
            this.scrolledwindow8.Name = "scrolledwindow8";
            this.scrolledwindow8.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow8.Gtk.Container+ContainerChild
            Gtk.Viewport w36 = new Gtk.Viewport();
            w36.ShadowType = ((Gtk.ShadowType)(0));
            // Container child GtkViewport.Gtk.Container+ContainerChild
            this.MarriageTreeView = new Gtk.TreeView();
            this.MarriageTreeView.CanFocus = true;
            this.MarriageTreeView.Name = "MarriageTreeView";
            w36.Add(this.MarriageTreeView);
            this.scrolledwindow8.Add(w36);
            this.table3.Add(this.scrolledwindow8);
            Gtk.Table.TableChild w39 = ((Gtk.Table.TableChild)(this.table3[this.scrolledwindow8]));
            w39.TopAttach = ((uint)(2));
            w39.BottomAttach = ((uint)(3));
            w39.RightAttach = ((uint)(5));
            w39.XOptions = ((Gtk.AttachOptions)(7));
            w39.YOptions = ((Gtk.AttachOptions)(7));
            this.Notebook.Add(this.table3);
            Gtk.Notebook.NotebookChild w40 = ((Gtk.Notebook.NotebookChild)(this.Notebook[this.table3]));
            w40.Position = 1;
            // Notebook tab
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.LabelProp = "Marriage";
            this.Notebook.SetTabLabel(this.table3, this.label6);
            this.label6.ShowAll();
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.AddressView = new Gedcom.UI.GTK.Widgets.AddressView();
            this.AddressView.Events = ((Gdk.EventMask)(256));
            this.AddressView.Name = "AddressView";
            this.Notebook.Add(this.AddressView);
            Gtk.Notebook.NotebookChild w41 = ((Gtk.Notebook.NotebookChild)(this.Notebook[this.AddressView]));
            w41.Position = 2;
            // Notebook tab
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.LabelProp = "Addresses";
            this.Notebook.SetTabLabel(this.AddressView, this.label3);
            this.label3.ShowAll();
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.table6 = new Gtk.Table(((uint)(4)), ((uint)(5)), false);
            this.table6.Name = "table6";
            this.table6.RowSpacing = ((uint)(6));
            this.table6.ColumnSpacing = ((uint)(12));
            this.table6.BorderWidth = ((uint)(6));
            // Container child table6.Gtk.Table+TableChild
            this.HeightEntry = new Gtk.Entry();
            this.HeightEntry.CanFocus = true;
            this.HeightEntry.Name = "HeightEntry";
            this.HeightEntry.IsEditable = true;
            this.HeightEntry.InvisibleChar = '●';
            this.table6.Add(this.HeightEntry);
            Gtk.Table.TableChild w42 = ((Gtk.Table.TableChild)(this.table6[this.HeightEntry]));
            w42.LeftAttach = ((uint)(1));
            w42.RightAttach = ((uint)(2));
            w42.XOptions = ((Gtk.AttachOptions)(4));
            w42.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table6.Gtk.Table+TableChild
            this.label30 = new Gtk.Label();
            this.label30.Name = "label30";
            this.label30.Xalign = 0F;
            this.label30.LabelProp = "Height:";
            this.table6.Add(this.label30);
            Gtk.Table.TableChild w43 = ((Gtk.Table.TableChild)(this.table6[this.label30]));
            w43.XOptions = ((Gtk.AttachOptions)(4));
            w43.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table6.Gtk.Table+TableChild
            this.label33 = new Gtk.Label();
            this.label33.Name = "label33";
            this.label33.Xalign = 0F;
            this.label33.LabelProp = "Weight:";
            this.table6.Add(this.label33);
            Gtk.Table.TableChild w44 = ((Gtk.Table.TableChild)(this.table6[this.label33]));
            w44.TopAttach = ((uint)(1));
            w44.BottomAttach = ((uint)(2));
            w44.XOptions = ((Gtk.AttachOptions)(4));
            w44.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table6.Gtk.Table+TableChild
            this.label34 = new Gtk.Label();
            this.label34.Name = "label34";
            this.label34.Xalign = 0F;
            this.label34.Yalign = 0F;
            this.label34.LabelProp = "Cause of Death:";
            this.table6.Add(this.label34);
            Gtk.Table.TableChild w45 = ((Gtk.Table.TableChild)(this.table6[this.label34]));
            w45.TopAttach = ((uint)(2));
            w45.BottomAttach = ((uint)(3));
            w45.XOptions = ((Gtk.AttachOptions)(4));
            w45.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table6.Gtk.Table+TableChild
            this.label35 = new Gtk.Label();
            this.label35.Name = "label35";
            this.label35.Xalign = 0F;
            this.label35.Yalign = 0F;
            this.label35.LabelProp = "Medical Information:";
            this.table6.Add(this.label35);
            Gtk.Table.TableChild w46 = ((Gtk.Table.TableChild)(this.table6[this.label35]));
            w46.TopAttach = ((uint)(3));
            w46.BottomAttach = ((uint)(4));
            w46.XOptions = ((Gtk.AttachOptions)(4));
            w46.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table6.Gtk.Table+TableChild
            this.scrolledwindow5 = new Gtk.ScrolledWindow();
            this.scrolledwindow5.CanFocus = true;
            this.scrolledwindow5.Name = "scrolledwindow5";
            this.scrolledwindow5.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow5.Gtk.Container+ContainerChild
            this.MedicalInformationTextView = new Gtk.TextView();
            this.MedicalInformationTextView.CanFocus = true;
            this.MedicalInformationTextView.Name = "MedicalInformationTextView";
            this.scrolledwindow5.Add(this.MedicalInformationTextView);
            this.table6.Add(this.scrolledwindow5);
            Gtk.Table.TableChild w48 = ((Gtk.Table.TableChild)(this.table6[this.scrolledwindow5]));
            w48.TopAttach = ((uint)(3));
            w48.BottomAttach = ((uint)(4));
            w48.LeftAttach = ((uint)(1));
            w48.RightAttach = ((uint)(5));
            // Container child table6.Gtk.Table+TableChild
            this.scrolledwindow6 = new Gtk.ScrolledWindow();
            this.scrolledwindow6.CanFocus = true;
            this.scrolledwindow6.Name = "scrolledwindow6";
            this.scrolledwindow6.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow6.Gtk.Container+ContainerChild
            this.CauseOfDeathTextView = new Gtk.TextView();
            this.CauseOfDeathTextView.CanFocus = true;
            this.CauseOfDeathTextView.Name = "CauseOfDeathTextView";
            this.scrolledwindow6.Add(this.CauseOfDeathTextView);
            this.table6.Add(this.scrolledwindow6);
            Gtk.Table.TableChild w50 = ((Gtk.Table.TableChild)(this.table6[this.scrolledwindow6]));
            w50.TopAttach = ((uint)(2));
            w50.BottomAttach = ((uint)(3));
            w50.LeftAttach = ((uint)(1));
            w50.RightAttach = ((uint)(5));
            // Container child table6.Gtk.Table+TableChild
            this.WeightEntry = new Gtk.Entry();
            this.WeightEntry.CanFocus = true;
            this.WeightEntry.Name = "WeightEntry";
            this.WeightEntry.IsEditable = true;
            this.WeightEntry.InvisibleChar = '●';
            this.table6.Add(this.WeightEntry);
            Gtk.Table.TableChild w51 = ((Gtk.Table.TableChild)(this.table6[this.WeightEntry]));
            w51.TopAttach = ((uint)(1));
            w51.BottomAttach = ((uint)(2));
            w51.LeftAttach = ((uint)(1));
            w51.RightAttach = ((uint)(2));
            w51.XOptions = ((Gtk.AttachOptions)(4));
            w51.YOptions = ((Gtk.AttachOptions)(4));
            this.Notebook.Add(this.table6);
            Gtk.Notebook.NotebookChild w52 = ((Gtk.Notebook.NotebookChild)(this.Notebook[this.table6]));
            w52.Position = 3;
            // Notebook tab
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.LabelProp = "Medical";
            this.Notebook.SetTabLabel(this.table6, this.label4);
            this.label4.ShowAll();
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.LabelProp = "label5";
            this.Notebook.Add(this.label5);
            Gtk.Notebook.NotebookChild w53 = ((Gtk.Notebook.NotebookChild)(this.Notebook[this.label5]));
            w53.Position = 4;
            // Notebook tab
            this.label40 = new Gtk.Label();
            this.label40.Name = "label40";
            this.label40.LabelProp = "Lineage";
            this.Notebook.SetTabLabel(this.label5, this.label40);
            this.label40.ShowAll();
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.NotesView = new Gedcom.UI.GTK.Widgets.NotesView();
            this.NotesView.Events = ((Gdk.EventMask)(256));
            this.NotesView.Name = "NotesView";
            this.NotesView.DataNotes = false;
            this.NotesView.ListOnly = false;
            this.NotesView.NoteOnly = false;
            this.Notebook.Add(this.NotesView);
            Gtk.Notebook.NotebookChild w54 = ((Gtk.Notebook.NotebookChild)(this.Notebook[this.NotesView]));
            w54.Position = 5;
            // Notebook tab
            this.label7 = new Gtk.Label();
            this.label7.Name = "label7";
            this.label7.LabelProp = "Notes";
            this.Notebook.SetTabLabel(this.NotesView, this.label7);
            this.label7.ShowAll();
            this.vbox1.Add(this.Notebook);
            Gtk.Box.BoxChild w55 = ((Gtk.Box.BoxChild)(this.vbox1[this.Notebook]));
            w55.Position = 0;
            // Container child vbox1.Gtk.Box+BoxChild
            this.vbox5 = new Gtk.VBox();
            this.vbox5.Name = "vbox5";
            // Container child vbox5.Gtk.Box+BoxChild
            this.SwitchBox = new Gtk.VBox();
            this.SwitchBox.Name = "SwitchBox";
            this.SwitchBox.Spacing = 6;
            // Container child SwitchBox.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.Xalign = 0F;
            this.label1.LabelProp = "Switch to this individual's:";
            this.SwitchBox.Add(this.label1);
            Gtk.Box.BoxChild w56 = ((Gtk.Box.BoxChild)(this.SwitchBox[this.label1]));
            w56.Position = 0;
            w56.Expand = false;
            w56.Fill = false;
            // Container child SwitchBox.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Homogeneous = true;
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.ParentsCombo = new Gedcom.UI.GTK.Widgets.IndividualListComboBox();
            this.ParentsCombo.Name = "ParentsCombo";
            this.ParentsCombo.Active = 0;
            this.ParentsCombo.NoIndividualText = "Parents";
            this.hbox3.Add(this.ParentsCombo);
            Gtk.Box.BoxChild w57 = ((Gtk.Box.BoxChild)(this.hbox3[this.ParentsCombo]));
            w57.Position = 0;
            // Container child hbox3.Gtk.Box+BoxChild
            this.ChildrenCombo = new Gedcom.UI.GTK.Widgets.IndividualListComboBox();
            this.ChildrenCombo.Name = "ChildrenCombo";
            this.ChildrenCombo.Active = 0;
            this.ChildrenCombo.NoIndividualText = "Children";
            this.hbox3.Add(this.ChildrenCombo);
            Gtk.Box.BoxChild w58 = ((Gtk.Box.BoxChild)(this.hbox3[this.ChildrenCombo]));
            w58.Position = 1;
            // Container child hbox3.Gtk.Box+BoxChild
            this.SpousesCombo = new Gedcom.UI.GTK.Widgets.IndividualListComboBox();
            this.SpousesCombo.Name = "SpousesCombo";
            this.SpousesCombo.Active = 0;
            this.SpousesCombo.NoIndividualText = "Spouses";
            this.hbox3.Add(this.SpousesCombo);
            Gtk.Box.BoxChild w59 = ((Gtk.Box.BoxChild)(this.hbox3[this.SpousesCombo]));
            w59.Position = 2;
            // Container child hbox3.Gtk.Box+BoxChild
            this.SiblingsCombo = new Gedcom.UI.GTK.Widgets.IndividualListComboBox();
            this.SiblingsCombo.Name = "SiblingsCombo";
            this.SiblingsCombo.Active = 0;
            this.SiblingsCombo.NoIndividualText = "Siblings";
            this.hbox3.Add(this.SiblingsCombo);
            Gtk.Box.BoxChild w60 = ((Gtk.Box.BoxChild)(this.hbox3[this.SiblingsCombo]));
            w60.Position = 3;
            this.SwitchBox.Add(this.hbox3);
            Gtk.Box.BoxChild w61 = ((Gtk.Box.BoxChild)(this.SwitchBox[this.hbox3]));
            w61.Position = 1;
            w61.Expand = false;
            w61.Fill = false;
            this.vbox5.Add(this.SwitchBox);
            Gtk.Box.BoxChild w62 = ((Gtk.Box.BoxChild)(this.vbox5[this.SwitchBox]));
            w62.Position = 0;
            w62.Expand = false;
            w62.Fill = false;
            this.vbox1.Add(this.vbox5);
            Gtk.Box.BoxChild w63 = ((Gtk.Box.BoxChild)(this.vbox1[this.vbox5]));
            w63.Position = 1;
            w63.Expand = false;
            w63.Fill = false;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.NameSourceButton.Clicked += new System.EventHandler(this.OnNameSourceButton_Clicked);
            this.NameButton.Clicked += new System.EventHandler(this.OnNameButton_Clicked);
            this.IndiScrapbookButton.Clicked += new System.EventHandler(this.OnIndiScrapbookButton_Clicked);
            this.FactView.MoreInformation += new System.EventHandler<Gedcom.UI.Common.FactArgs>(this.OnFactView_MoreInformation);
            this.DiedInEntry.Changed += new System.EventHandler(this.Died_Changed);
            this.DeleteButton.Clicked += new System.EventHandler(this.OnDeleteButton_Clicked);
            this.DateDiedSourceButton.Clicked += new System.EventHandler(this.OnDateDiedSourceButton_Clicked);
            this.DateDiedEntry.Changed += new System.EventHandler(this.Died_Changed);
            this.DateBornSourceButton.Clicked += new System.EventHandler(this.OnDateBornSourceButton_Clicked);
            this.DateBornEntry.Changed += new System.EventHandler(this.Born_Changed);
            this.BornInEntry.Changed += new System.EventHandler(this.Born_Changed);
            this.AddressView.ShowSourceCitation += new System.EventHandler<Gedcom.UI.Common.SourceCitationArgs>(this.OnAddressView_ShowSourceCitation);
            this.AddressView.ShowScrapBook += new System.EventHandler<Gedcom.UI.Common.ScrapBookArgs>(this.OnAddressView_ShowScrapBook);
            this.AddressView.MoreFactInformation += new System.EventHandler<Gedcom.UI.Common.FactArgs>(this.OnAddressView_MoreFactInformation);
            this.NotesView.ShowSourceCitation += new System.EventHandler<Gedcom.UI.Common.SourceCitationArgs>(this.OnNotesView_ShowSourceCitation);
            this.NotesView.SelectNewNote += new System.EventHandler<Gedcom.UI.Common.NoteArgs>(this.OnNotesView_SelectNewNote);
            this.ParentsCombo.Changed += new System.EventHandler(this.OnParentsCombo_Changed);
            this.ChildrenCombo.Changed += new System.EventHandler(this.OnChildrenCombo_Changed);
            this.SpousesCombo.Changed += new System.EventHandler(this.OnSpousesCombo_Changed);
            this.SiblingsCombo.Changed += new System.EventHandler(this.OnSiblingsCombo_Changed);
        }
    }
}