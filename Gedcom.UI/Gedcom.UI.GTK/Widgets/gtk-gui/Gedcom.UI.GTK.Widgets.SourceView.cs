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
    
    
    public partial class SourceView {
        
        private Gtk.Notebook Notebook;
        
        private Gtk.Table table1;
        
        private Gtk.Entry FiledByEntry;
        
        private Gtk.HBox hbox4;
        
        private Gtk.Button ScrapBookButton;
        
        private Gtk.Label label10;
        
        private Gtk.Label label18;
        
        private Gtk.Label label4;
        
        private Gtk.Label label8;
        
        private Gtk.Entry OriginatorEntry;
        
        private Gtk.ScrolledWindow scrolledwindow5;
        
        private Gtk.TextView PublicationFactsTextView;
        
        private Gtk.Entry TitleEntry;
        
        private Gtk.Label label5;
        
        private Gtk.Table table2;
        
        private Gtk.Entry DateRecordedEntry;
        
        private Gtk.HBox hbox3;
        
        private Gtk.ScrolledWindow scrolledwindow6;
        
        private Gtk.TreeView EventGroupTreeView;
        
        private Gtk.VButtonBox vbuttonbox1;
        
        private Gtk.Button NewEventGroupButton;
        
        private Gtk.HSeparator hseparator1;
        
        private Gtk.Label label1;
        
        private Gtk.Label label16;
        
        private Gtk.Label label17;
        
        private Gtk.Label label2;
        
        private Gtk.Entry PlaceRecordedEntry;
        
        private Gtk.ScrolledWindow scrolledwindow9;
        
        private Gtk.TreeView EventTypeTreeView;
        
        private Gtk.Label label6;
        
        private Gtk.Table table3;
        
        private Gtk.Entry AgencyTextBox;
        
        private Gedcom.UI.GTK.Widgets.NotesView DataNotesView;
        
        private Gtk.Label label3;
        
        private Gtk.Label label7;
        
        private Gtk.ScrolledWindow scrolledwindow2;
        
        private Gtk.TextView TextTextView;
        
        private Gtk.Label label15;
        
        private Gedcom.UI.GTK.Widgets.NotesView NotesView;
        
        private Gtk.Label label9;
        
        private Gtk.Table table4;
        
        private Gtk.Entry CallNumberEntry;
        
        private Gtk.Label label12;
        
        private Gtk.Label label13;
        
        private Gtk.Label label14;
        
        private Gtk.ComboBox MediaTypeCombo;
        
        private Gedcom.UI.GTK.Widgets.NotesView RepoNotesView;
        
        private Gtk.ScrolledWindow scrolledwindow4;
        
        private Gtk.TreeView CallNumberTreeView;
        
        private Gtk.Label label11;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget Gedcom.UI.GTK.Widgets.SourceView
            Stetic.BinContainer.Attach(this);
            this.Events = ((Gdk.EventMask)(256));
            this.Name = "Gedcom.UI.GTK.Widgets.SourceView";
            // Container child Gedcom.UI.GTK.Widgets.SourceView.Gtk.Container+ContainerChild
            this.Notebook = new Gtk.Notebook();
            this.Notebook.CanFocus = true;
            this.Notebook.Name = "Notebook";
            this.Notebook.CurrentPage = 2;
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.table1 = new Gtk.Table(((uint)(6)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(12));
            this.table1.BorderWidth = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.FiledByEntry = new Gtk.Entry();
            this.FiledByEntry.CanFocus = true;
            this.FiledByEntry.Name = "FiledByEntry";
            this.FiledByEntry.IsEditable = true;
            this.FiledByEntry.InvisibleChar = '●';
            this.table1.Add(this.FiledByEntry);
            Gtk.Table.TableChild w1 = ((Gtk.Table.TableChild)(this.table1[this.FiledByEntry]));
            w1.TopAttach = ((uint)(2));
            w1.BottomAttach = ((uint)(3));
            w1.LeftAttach = ((uint)(1));
            w1.RightAttach = ((uint)(2));
            w1.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Spacing = 6;
            // Container child hbox4.Gtk.Box+BoxChild
            this.ScrapBookButton = new Gtk.Button();
            this.ScrapBookButton.CanFocus = true;
            this.ScrapBookButton.Name = "ScrapBookButton";
            // Container child ScrapBookButton.Gtk.Container+ContainerChild
            Gtk.Alignment w2 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w3 = new Gtk.HBox();
            w3.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w4 = new Gtk.Image();
            w4.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-paste", Gtk.IconSize.Button, 20);
            w3.Add(w4);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w6 = new Gtk.Label();
            w6.LabelProp = "Scrapbook";
            w3.Add(w6);
            w2.Add(w3);
            this.ScrapBookButton.Add(w2);
            this.hbox4.Add(this.ScrapBookButton);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.hbox4[this.ScrapBookButton]));
            w10.PackType = ((Gtk.PackType)(1));
            w10.Position = 1;
            w10.Expand = false;
            w10.Fill = false;
            this.table1.Add(this.hbox4);
            Gtk.Table.TableChild w11 = ((Gtk.Table.TableChild)(this.table1[this.hbox4]));
            w11.TopAttach = ((uint)(3));
            w11.BottomAttach = ((uint)(4));
            w11.RightAttach = ((uint)(2));
            w11.XOptions = ((Gtk.AttachOptions)(4));
            w11.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label10 = new Gtk.Label();
            this.label10.Name = "label10";
            this.label10.Xalign = 0F;
            this.label10.LabelProp = "Publication Facts:";
            this.table1.Add(this.label10);
            Gtk.Table.TableChild w12 = ((Gtk.Table.TableChild)(this.table1[this.label10]));
            w12.TopAttach = ((uint)(4));
            w12.BottomAttach = ((uint)(5));
            w12.XOptions = ((Gtk.AttachOptions)(4));
            w12.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label18 = new Gtk.Label();
            this.label18.Name = "label18";
            this.label18.Xalign = 0F;
            this.label18.LabelProp = "Author:";
            this.table1.Add(this.label18);
            Gtk.Table.TableChild w13 = ((Gtk.Table.TableChild)(this.table1[this.label18]));
            w13.TopAttach = ((uint)(1));
            w13.BottomAttach = ((uint)(2));
            w13.XOptions = ((Gtk.AttachOptions)(4));
            w13.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.Xalign = 0F;
            this.label4.LabelProp = "Title:";
            this.table1.Add(this.label4);
            Gtk.Table.TableChild w14 = ((Gtk.Table.TableChild)(this.table1[this.label4]));
            w14.XOptions = ((Gtk.AttachOptions)(4));
            w14.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.Xalign = 0F;
            this.label8.LabelProp = "Filed By:";
            this.table1.Add(this.label8);
            Gtk.Table.TableChild w15 = ((Gtk.Table.TableChild)(this.table1[this.label8]));
            w15.TopAttach = ((uint)(2));
            w15.BottomAttach = ((uint)(3));
            w15.XOptions = ((Gtk.AttachOptions)(4));
            w15.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.OriginatorEntry = new Gtk.Entry();
            this.OriginatorEntry.CanFocus = true;
            this.OriginatorEntry.Name = "OriginatorEntry";
            this.OriginatorEntry.IsEditable = true;
            this.OriginatorEntry.InvisibleChar = '●';
            this.table1.Add(this.OriginatorEntry);
            Gtk.Table.TableChild w16 = ((Gtk.Table.TableChild)(this.table1[this.OriginatorEntry]));
            w16.TopAttach = ((uint)(1));
            w16.BottomAttach = ((uint)(2));
            w16.LeftAttach = ((uint)(1));
            w16.RightAttach = ((uint)(2));
            w16.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.scrolledwindow5 = new Gtk.ScrolledWindow();
            this.scrolledwindow5.CanFocus = true;
            this.scrolledwindow5.Name = "scrolledwindow5";
            this.scrolledwindow5.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow5.Gtk.Container+ContainerChild
            this.PublicationFactsTextView = new Gtk.TextView();
            this.PublicationFactsTextView.CanFocus = true;
            this.PublicationFactsTextView.Name = "PublicationFactsTextView";
            this.scrolledwindow5.Add(this.PublicationFactsTextView);
            this.table1.Add(this.scrolledwindow5);
            Gtk.Table.TableChild w18 = ((Gtk.Table.TableChild)(this.table1[this.scrolledwindow5]));
            w18.TopAttach = ((uint)(5));
            w18.BottomAttach = ((uint)(6));
            w18.RightAttach = ((uint)(2));
            // Container child table1.Gtk.Table+TableChild
            this.TitleEntry = new Gtk.Entry();
            this.TitleEntry.CanFocus = true;
            this.TitleEntry.Name = "TitleEntry";
            this.TitleEntry.IsEditable = true;
            this.TitleEntry.InvisibleChar = '●';
            this.table1.Add(this.TitleEntry);
            Gtk.Table.TableChild w19 = ((Gtk.Table.TableChild)(this.table1[this.TitleEntry]));
            w19.LeftAttach = ((uint)(1));
            w19.RightAttach = ((uint)(2));
            w19.XOptions = ((Gtk.AttachOptions)(4));
            w19.YOptions = ((Gtk.AttachOptions)(4));
            this.Notebook.Add(this.table1);
            // Notebook tab
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.LabelProp = "Details";
            this.Notebook.SetTabLabel(this.table1, this.label5);
            this.label5.ShowAll();
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.table2 = new Gtk.Table(((uint)(6)), ((uint)(4)), false);
            this.table2.Name = "table2";
            this.table2.RowSpacing = ((uint)(6));
            this.table2.ColumnSpacing = ((uint)(12));
            this.table2.BorderWidth = ((uint)(6));
            // Container child table2.Gtk.Table+TableChild
            this.DateRecordedEntry = new Gtk.Entry();
            this.DateRecordedEntry.CanFocus = true;
            this.DateRecordedEntry.Name = "DateRecordedEntry";
            this.DateRecordedEntry.IsEditable = true;
            this.DateRecordedEntry.InvisibleChar = '●';
            this.table2.Add(this.DateRecordedEntry);
            Gtk.Table.TableChild w21 = ((Gtk.Table.TableChild)(this.table2[this.DateRecordedEntry]));
            w21.TopAttach = ((uint)(2));
            w21.BottomAttach = ((uint)(3));
            w21.LeftAttach = ((uint)(1));
            w21.RightAttach = ((uint)(2));
            w21.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.scrolledwindow6 = new Gtk.ScrolledWindow();
            this.scrolledwindow6.CanFocus = true;
            this.scrolledwindow6.Name = "scrolledwindow6";
            this.scrolledwindow6.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow6.Gtk.Container+ContainerChild
            Gtk.Viewport w22 = new Gtk.Viewport();
            w22.ShadowType = ((Gtk.ShadowType)(0));
            // Container child GtkViewport.Gtk.Container+ContainerChild
            this.EventGroupTreeView = new Gtk.TreeView();
            this.EventGroupTreeView.CanFocus = true;
            this.EventGroupTreeView.Name = "EventGroupTreeView";
            w22.Add(this.EventGroupTreeView);
            this.scrolledwindow6.Add(w22);
            this.hbox3.Add(this.scrolledwindow6);
            Gtk.Box.BoxChild w25 = ((Gtk.Box.BoxChild)(this.hbox3[this.scrolledwindow6]));
            w25.Position = 0;
            // Container child hbox3.Gtk.Box+BoxChild
            this.vbuttonbox1 = new Gtk.VButtonBox();
            this.vbuttonbox1.Name = "vbuttonbox1";
            this.vbuttonbox1.Spacing = 6;
            this.vbuttonbox1.LayoutStyle = ((Gtk.ButtonBoxStyle)(3));
            // Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.NewEventGroupButton = new Gtk.Button();
            this.NewEventGroupButton.CanFocus = true;
            this.NewEventGroupButton.Name = "NewEventGroupButton";
            this.NewEventGroupButton.UseStock = true;
            this.NewEventGroupButton.UseUnderline = true;
            this.NewEventGroupButton.Label = "gtk-new";
            this.vbuttonbox1.Add(this.NewEventGroupButton);
            Gtk.ButtonBox.ButtonBoxChild w26 = ((Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.NewEventGroupButton]));
            w26.Expand = false;
            w26.Fill = false;
            this.hbox3.Add(this.vbuttonbox1);
            Gtk.Box.BoxChild w27 = ((Gtk.Box.BoxChild)(this.hbox3[this.vbuttonbox1]));
            w27.Position = 1;
            w27.Expand = false;
            w27.Fill = false;
            this.table2.Add(this.hbox3);
            Gtk.Table.TableChild w28 = ((Gtk.Table.TableChild)(this.table2[this.hbox3]));
            w28.TopAttach = ((uint)(5));
            w28.BottomAttach = ((uint)(6));
            w28.RightAttach = ((uint)(4));
            // Container child table2.Gtk.Table+TableChild
            this.hseparator1 = new Gtk.HSeparator();
            this.hseparator1.Name = "hseparator1";
            this.table2.Add(this.hseparator1);
            Gtk.Table.TableChild w29 = ((Gtk.Table.TableChild)(this.table2[this.hseparator1]));
            w29.TopAttach = ((uint)(3));
            w29.BottomAttach = ((uint)(4));
            w29.RightAttach = ((uint)(4));
            w29.XOptions = ((Gtk.AttachOptions)(4));
            w29.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.Xalign = 0F;
            this.label1.LabelProp = "Event Groups:";
            this.table2.Add(this.label1);
            Gtk.Table.TableChild w30 = ((Gtk.Table.TableChild)(this.table2[this.label1]));
            w30.TopAttach = ((uint)(4));
            w30.BottomAttach = ((uint)(5));
            w30.XOptions = ((Gtk.AttachOptions)(4));
            w30.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label16 = new Gtk.Label();
            this.label16.Name = "label16";
            this.label16.Xalign = 0F;
            this.label16.LabelProp = "Date Recorded:";
            this.table2.Add(this.label16);
            Gtk.Table.TableChild w31 = ((Gtk.Table.TableChild)(this.table2[this.label16]));
            w31.TopAttach = ((uint)(2));
            w31.BottomAttach = ((uint)(3));
            w31.XOptions = ((Gtk.AttachOptions)(4));
            w31.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label17 = new Gtk.Label();
            this.label17.Name = "label17";
            this.label17.Xalign = 0F;
            this.label17.LabelProp = "Events in selected group:";
            this.table2.Add(this.label17);
            Gtk.Table.TableChild w32 = ((Gtk.Table.TableChild)(this.table2[this.label17]));
            w32.RightAttach = ((uint)(4));
            w32.XOptions = ((Gtk.AttachOptions)(4));
            w32.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = "in";
            this.table2.Add(this.label2);
            Gtk.Table.TableChild w33 = ((Gtk.Table.TableChild)(this.table2[this.label2]));
            w33.TopAttach = ((uint)(2));
            w33.BottomAttach = ((uint)(3));
            w33.LeftAttach = ((uint)(2));
            w33.RightAttach = ((uint)(3));
            w33.XOptions = ((Gtk.AttachOptions)(4));
            w33.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.PlaceRecordedEntry = new Gtk.Entry();
            this.PlaceRecordedEntry.CanFocus = true;
            this.PlaceRecordedEntry.Name = "PlaceRecordedEntry";
            this.PlaceRecordedEntry.IsEditable = true;
            this.PlaceRecordedEntry.InvisibleChar = '●';
            this.table2.Add(this.PlaceRecordedEntry);
            Gtk.Table.TableChild w34 = ((Gtk.Table.TableChild)(this.table2[this.PlaceRecordedEntry]));
            w34.TopAttach = ((uint)(2));
            w34.BottomAttach = ((uint)(3));
            w34.LeftAttach = ((uint)(3));
            w34.RightAttach = ((uint)(4));
            w34.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table2.Gtk.Table+TableChild
            this.scrolledwindow9 = new Gtk.ScrolledWindow();
            this.scrolledwindow9.CanFocus = true;
            this.scrolledwindow9.Name = "scrolledwindow9";
            this.scrolledwindow9.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow9.Gtk.Container+ContainerChild
            Gtk.Viewport w35 = new Gtk.Viewport();
            w35.ShadowType = ((Gtk.ShadowType)(0));
            // Container child GtkViewport1.Gtk.Container+ContainerChild
            this.EventTypeTreeView = new Gtk.TreeView();
            this.EventTypeTreeView.CanFocus = true;
            this.EventTypeTreeView.Name = "EventTypeTreeView";
            this.EventTypeTreeView.RulesHint = true;
            w35.Add(this.EventTypeTreeView);
            this.scrolledwindow9.Add(w35);
            this.table2.Add(this.scrolledwindow9);
            Gtk.Table.TableChild w38 = ((Gtk.Table.TableChild)(this.table2[this.scrolledwindow9]));
            w38.TopAttach = ((uint)(1));
            w38.BottomAttach = ((uint)(2));
            w38.RightAttach = ((uint)(4));
            w38.XOptions = ((Gtk.AttachOptions)(4));
            this.Notebook.Add(this.table2);
            Gtk.Notebook.NotebookChild w39 = ((Gtk.Notebook.NotebookChild)(this.Notebook[this.table2]));
            w39.Position = 1;
            // Notebook tab
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.LabelProp = "Events Recorded";
            this.Notebook.SetTabLabel(this.table2, this.label6);
            this.label6.ShowAll();
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.table3 = new Gtk.Table(((uint)(2)), ((uint)(2)), false);
            this.table3.Name = "table3";
            this.table3.RowSpacing = ((uint)(6));
            this.table3.ColumnSpacing = ((uint)(12));
            this.table3.BorderWidth = ((uint)(6));
            // Container child table3.Gtk.Table+TableChild
            this.AgencyTextBox = new Gtk.Entry();
            this.AgencyTextBox.CanFocus = true;
            this.AgencyTextBox.Name = "AgencyTextBox";
            this.AgencyTextBox.IsEditable = true;
            this.AgencyTextBox.InvisibleChar = '●';
            this.table3.Add(this.AgencyTextBox);
            Gtk.Table.TableChild w40 = ((Gtk.Table.TableChild)(this.table3[this.AgencyTextBox]));
            w40.LeftAttach = ((uint)(1));
            w40.RightAttach = ((uint)(2));
            w40.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.DataNotesView = new Gedcom.UI.GTK.Widgets.NotesView();
            this.DataNotesView.Events = ((Gdk.EventMask)(256));
            this.DataNotesView.Name = "DataNotesView";
            this.DataNotesView.DataNotes = true;
            this.DataNotesView.ListOnly = false;
            this.DataNotesView.NoteOnly = false;
            this.table3.Add(this.DataNotesView);
            Gtk.Table.TableChild w41 = ((Gtk.Table.TableChild)(this.table3[this.DataNotesView]));
            w41.TopAttach = ((uint)(1));
            w41.BottomAttach = ((uint)(2));
            w41.RightAttach = ((uint)(2));
            // Container child table3.Gtk.Table+TableChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.Xalign = 0F;
            this.label3.LabelProp = "Agency:";
            this.table3.Add(this.label3);
            Gtk.Table.TableChild w42 = ((Gtk.Table.TableChild)(this.table3[this.label3]));
            w42.XOptions = ((Gtk.AttachOptions)(4));
            w42.YOptions = ((Gtk.AttachOptions)(4));
            this.Notebook.Add(this.table3);
            Gtk.Notebook.NotebookChild w43 = ((Gtk.Notebook.NotebookChild)(this.Notebook[this.table3]));
            w43.Position = 2;
            // Notebook tab
            this.label7 = new Gtk.Label();
            this.label7.Name = "label7";
            this.label7.LabelProp = "Data Information";
            this.Notebook.SetTabLabel(this.table3, this.label7);
            this.label7.ShowAll();
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.scrolledwindow2 = new Gtk.ScrolledWindow();
            this.scrolledwindow2.CanFocus = true;
            this.scrolledwindow2.Name = "scrolledwindow2";
            this.scrolledwindow2.ShadowType = ((Gtk.ShadowType)(1));
            this.scrolledwindow2.BorderWidth = ((uint)(6));
            // Container child scrolledwindow2.Gtk.Container+ContainerChild
            this.TextTextView = new Gtk.TextView();
            this.TextTextView.CanFocus = true;
            this.TextTextView.Name = "TextTextView";
            this.scrolledwindow2.Add(this.TextTextView);
            this.Notebook.Add(this.scrolledwindow2);
            Gtk.Notebook.NotebookChild w45 = ((Gtk.Notebook.NotebookChild)(this.Notebook[this.scrolledwindow2]));
            w45.Position = 3;
            // Notebook tab
            this.label15 = new Gtk.Label();
            this.label15.Name = "label15";
            this.label15.LabelProp = "Text";
            this.Notebook.SetTabLabel(this.scrolledwindow2, this.label15);
            this.label15.ShowAll();
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.NotesView = new Gedcom.UI.GTK.Widgets.NotesView();
            this.NotesView.Events = ((Gdk.EventMask)(256));
            this.NotesView.Name = "NotesView";
            this.NotesView.DataNotes = false;
            this.NotesView.ListOnly = false;
            this.NotesView.NoteOnly = false;
            this.Notebook.Add(this.NotesView);
            Gtk.Notebook.NotebookChild w46 = ((Gtk.Notebook.NotebookChild)(this.Notebook[this.NotesView]));
            w46.Position = 4;
            // Notebook tab
            this.label9 = new Gtk.Label();
            this.label9.Name = "label9";
            this.label9.LabelProp = "Notes";
            this.Notebook.SetTabLabel(this.NotesView, this.label9);
            this.label9.ShowAll();
            // Container child Notebook.Gtk.Notebook+NotebookChild
            this.table4 = new Gtk.Table(((uint)(4)), ((uint)(4)), false);
            this.table4.Name = "table4";
            this.table4.RowSpacing = ((uint)(6));
            this.table4.ColumnSpacing = ((uint)(12));
            this.table4.BorderWidth = ((uint)(6));
            // Container child table4.Gtk.Table+TableChild
            this.CallNumberEntry = new Gtk.Entry();
            this.CallNumberEntry.CanFocus = true;
            this.CallNumberEntry.Name = "CallNumberEntry";
            this.CallNumberEntry.IsEditable = true;
            this.CallNumberEntry.InvisibleChar = '●';
            this.table4.Add(this.CallNumberEntry);
            Gtk.Table.TableChild w47 = ((Gtk.Table.TableChild)(this.table4[this.CallNumberEntry]));
            w47.LeftAttach = ((uint)(1));
            w47.RightAttach = ((uint)(2));
            w47.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table4.Gtk.Table+TableChild
            this.label12 = new Gtk.Label();
            this.label12.Name = "label12";
            this.label12.Xalign = 0F;
            this.label12.LabelProp = "Call Number:";
            this.table4.Add(this.label12);
            Gtk.Table.TableChild w48 = ((Gtk.Table.TableChild)(this.table4[this.label12]));
            w48.XOptions = ((Gtk.AttachOptions)(4));
            w48.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table4.Gtk.Table+TableChild
            this.label13 = new Gtk.Label();
            this.label13.Name = "label13";
            this.label13.Xalign = 0F;
            this.label13.LabelProp = "Media Type:";
            this.table4.Add(this.label13);
            Gtk.Table.TableChild w49 = ((Gtk.Table.TableChild)(this.table4[this.label13]));
            w49.LeftAttach = ((uint)(2));
            w49.RightAttach = ((uint)(3));
            w49.XOptions = ((Gtk.AttachOptions)(4));
            w49.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table4.Gtk.Table+TableChild
            this.label14 = new Gtk.Label();
            this.label14.Name = "label14";
            this.label14.Xalign = 0F;
            this.label14.LabelProp = "Notes:";
            this.table4.Add(this.label14);
            Gtk.Table.TableChild w50 = ((Gtk.Table.TableChild)(this.table4[this.label14]));
            w50.TopAttach = ((uint)(2));
            w50.BottomAttach = ((uint)(3));
            w50.XOptions = ((Gtk.AttachOptions)(4));
            w50.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table4.Gtk.Table+TableChild
            this.MediaTypeCombo = new Gtk.ComboBox();
            this.MediaTypeCombo.Name = "MediaTypeCombo";
            this.table4.Add(this.MediaTypeCombo);
            Gtk.Table.TableChild w51 = ((Gtk.Table.TableChild)(this.table4[this.MediaTypeCombo]));
            w51.LeftAttach = ((uint)(3));
            w51.RightAttach = ((uint)(4));
            w51.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table4.Gtk.Table+TableChild
            this.RepoNotesView = new Gedcom.UI.GTK.Widgets.NotesView();
            this.RepoNotesView.Events = ((Gdk.EventMask)(256));
            this.RepoNotesView.Name = "RepoNotesView";
            this.RepoNotesView.DataNotes = false;
            this.RepoNotesView.ListOnly = false;
            this.RepoNotesView.NoteOnly = false;
            this.table4.Add(this.RepoNotesView);
            Gtk.Table.TableChild w52 = ((Gtk.Table.TableChild)(this.table4[this.RepoNotesView]));
            w52.TopAttach = ((uint)(3));
            w52.BottomAttach = ((uint)(4));
            w52.RightAttach = ((uint)(4));
            w52.XOptions = ((Gtk.AttachOptions)(4));
            w52.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table4.Gtk.Table+TableChild
            this.scrolledwindow4 = new Gtk.ScrolledWindow();
            this.scrolledwindow4.CanFocus = true;
            this.scrolledwindow4.Name = "scrolledwindow4";
            this.scrolledwindow4.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow4.Gtk.Container+ContainerChild
            this.CallNumberTreeView = new Gtk.TreeView();
            this.CallNumberTreeView.CanFocus = true;
            this.CallNumberTreeView.Name = "CallNumberTreeView";
            this.scrolledwindow4.Add(this.CallNumberTreeView);
            this.table4.Add(this.scrolledwindow4);
            Gtk.Table.TableChild w54 = ((Gtk.Table.TableChild)(this.table4[this.scrolledwindow4]));
            w54.TopAttach = ((uint)(1));
            w54.BottomAttach = ((uint)(2));
            w54.RightAttach = ((uint)(4));
            this.Notebook.Add(this.table4);
            Gtk.Notebook.NotebookChild w55 = ((Gtk.Notebook.NotebookChild)(this.Notebook[this.table4]));
            w55.Position = 5;
            // Notebook tab
            this.label11 = new Gtk.Label();
            this.label11.Name = "label11";
            this.label11.LabelProp = "Repositories";
            this.Notebook.SetTabLabel(this.table4, this.label11);
            this.label11.ShowAll();
            this.Add(this.Notebook);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.Notebook.SwitchPage += new Gtk.SwitchPageHandler(this.OnNotebook_SwitchPage);
            this.ScrapBookButton.Clicked += new System.EventHandler(this.OnScrapbookButton_Clicked);
            this.EventGroupTreeView.ButtonPressEvent += new Gtk.ButtonPressEventHandler(this.OnEventGroupTreeView_ButtonPressEvent);
            this.NewEventGroupButton.Clicked += new System.EventHandler(this.OnNewEventGroupButton_Clicked);
            this.NotesView.ShowSourceCitation += new System.EventHandler<Gedcom.UI.Common.SourceCitationArgs>(this.OnNotesView_ShowSourceCitation);
            this.NotesView.SelectNewNote += new System.EventHandler<Gedcom.UI.Common.NoteArgs>(this.OnNotesView_SelectNewNote);
        }
    }
}
