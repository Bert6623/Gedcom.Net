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
    
    
    public partial class PropertiesDialog {
        
        private Gedcom.UI.GTK.Widgets.HeaderView HeaderView;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget Gedcom.UI.GTK.PropertiesDialog
            this.Name = "Gedcom.UI.GTK.PropertiesDialog";
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.HasSeparator = false;
            // Internal child Gedcom.UI.GTK.PropertiesDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.HeaderView = new Gedcom.UI.GTK.Widgets.HeaderView();
            this.HeaderView.Events = ((Gdk.EventMask)(256));
            this.HeaderView.Name = "HeaderView";
            w1.Add(this.HeaderView);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(w1[this.HeaderView]));
            w2.Position = 0;
            // Internal child Gedcom.UI.GTK.PropertiesDialog.ActionArea
            Gtk.HButtonBox w3 = this.ActionArea;
            w3.Name = "dialog1_ActionArea";
            w3.Spacing = 6;
            w3.BorderWidth = ((uint)(5));
            w3.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-close";
            this.AddActionWidget(this.buttonOk, -7);
            Gtk.ButtonBox.ButtonBoxChild w4 = ((Gtk.ButtonBox.ButtonBoxChild)(w3[this.buttonOk]));
            w4.Expand = false;
            w4.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 300;
            this.Show();
            this.HeaderView.ShowSourceCitation += new System.EventHandler<Gedcom.UI.Common.SourceCitationArgs>(this.OnHeaderView_ShowSourceCitation);
            this.HeaderView.SelectNewNote += new System.EventHandler<Gedcom.UI.Common.NoteArgs>(this.OnHeaderView_SelectNewNote);
            this.HeaderView.OpenFile += new System.EventHandler<Gedcom.UI.Common.MultimediaFileArgs>(this.OnHeaderView_OpenFile);
            this.HeaderView.AddFile += new System.EventHandler<Gedcom.UI.Common.MultimediaFileArgs>(this.OnHeaderView_AddFile);
        }
    }
}