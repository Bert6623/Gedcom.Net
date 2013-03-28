main();

function main()
{
  if (WScript.Arguments.length < 1) 
  {
    WScript.echo("usage: validate instance_file");
	return;
  }

  var fname = WScript.Arguments.Item(0);
  var dom = new ActiveXObject("MSXML2.DOMDocument.4.0");
  dom.async = false;

  if (dom.load(fname)) 
	WScript.echo("success: document conforms to DTD/Schema");
  else
  {
	  WScript.echo("### error: (" + dom.parseError.line + ", " + dom.parseError.linepos + ") " + dom.parseError.reason);
	  WScript.echo("### source: " + dom.parseError.srcText);
  }
}

