<%@ Control Language="C#" AutoEventWireup="true" Inherits="Contoso.PartnerPortal.ContextualHelp.ContextualHelpControl, Contoso.PartnerPortal, Culture=neutral, Version=1.0.0.0, PublicKeyToken=0561205e6cefaed4" %>
<script type="text/javascript">
    function changeVisibility() 
    {
        if (document.getElementById("showHelp").checked)
            document.getElementById("literalPlaceHolder").style.display = "block";
        else
            document.getElementById("literalPlaceHolder").style.display = "none";
    }
</script>
<div>
    <input id="showHelp" type="checkbox" onclick="changeVisibility()" name="showhelp" checked="checked" value="Show Help" />Show Help
</div>
<div id="literalPlaceHolder" style="margin: 0 5 5 0; padding: 5 5 5 5">
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
</div>