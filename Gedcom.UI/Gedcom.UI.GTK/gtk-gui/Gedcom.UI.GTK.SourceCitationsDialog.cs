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
    
    
    public partial class SourceCitationsDialog {
        
        private Gtk.VBox vbox2;
        
        private Gedcom.UI.GTK.Widgets.SourceCitationView SourceCitationView;
        
        private Gedcom.UI.GTK.Widgets.SourceCitationList SourceCitationList;
        
        private Gtk.Button CloseButton;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget Gedcom.UI.GTK.SourceCitationsDialog
            this.Events = ((Gdk.EventMask)(256));
            this.Name = "Gedcom.UI.GTK.SourceCitationsDialog";
            this.Title = Mono.Unix.Catalog.GetString("SourceCitationsDialog");
            this.TypeHint = ((Gdk.WindowTypeHint)(1));
            this.HasSeparator = false;
            // Internal child Gedcom.UI.GTK.SourceCitationsDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Events = ((Gdk.EventMask)(256));
            w1.Name = "dialog_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog_VBox.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            this.vbox2.BorderWidth = ((uint)(6));
            // Container child vbox2.Gtk.Box+BoxChild
            this.SourceCitationView = new Gedcom.UI.GTK.Widgets.SourceCitationView();
            this.SourceCitationView.Events = ((Gdk.EventMask)(256));
            this.SourceCitationView.Name = "SourceCitationView";
            this.vbox2.Add(this.SourceCitationView);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox2[this.SourceCitationView]));
            w2.Position = 0;
            // Container child vbox2.Gtk.Box+BoxChild
            this.SourceCitationList = new Gedcom.UI.GTK.Widgets.SourceCitationList();
            this.SourceCitationList.Events = ((Gdk.EventMask)(256));
            this.SourceCitationList.Name = "SourceCitationList";
            this.vbox2.Add(this.SourceCitationList);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox2[this.SourceCitationList]));
            w3.Position = 1;
            w1.Add(this.vbox2);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(w1[this.vbox2]));
            w4.Position = 0;
            // Internal child Gedcom.UI.GTK.SourceCitationsDialog.ActionArea
            Gtk.HButtonBox w5 = this.ActionArea;
            w5.Name = "GtkDialog_ActionArea";
            w5.Spacing = 6;
            w5.BorderWidth = ((uint)(5));
            w5.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child GtkDialog_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.CloseButton = new Gtk.Button();
            this.CloseButton.CanFocus = true;
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.UseStock = true;
            this.CloseButton.UseUnderline = true;
            this.CloseButton.Label = "gtk-close";
            this.AddActionWidget(this.CloseButton, -7);
            Gtk.ButtonBox.ButtonBoxChild w6 = ((Gtk.ButtonBox.ButtonBoxChild)(w5[this.CloseButton]));
            w6.Expand = false;
            w6.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 654;
            this.DefaultHeight = 484;
            this.Show();
            this.SourceCitationView.ViewMasterSource += new System.EventHandler(this.OnViewMasterSource);
            this.SourceCitationView.SelectMasterSource += new System.EventHandler<Gedcom.UI.Common.SourceArgs>(this.OnSelectMasterSource);
            this.SourceCitationView.ScrapBookButtonClicked += new System.EventHandler(this.OnScrapBookButtonClicked);
            this.SourceCitationView.SelectNewNote += new System.EventHandler<Gedcom.UI.Common.NoteArgs>(this.OnSourceCitationView_SelectNewNote);
            this.SourceCitationView.ShowSourceCitation += new System.EventHandler<Gedcom.UI.Common.SourceCitationArgs>(this.OnSourceCitationView_ShowSourceCitation);
        }
    }
}
