<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoteUsers.aspx.cs" Inherits="VoteForUsers.Layouts.VoteForUsers.VoteUsers" DynamicMasterPageFile="~masterurl/default.master" %>



<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:ScriptLink runat="server"  Name="script/voteJS.js" LoadAfterUI="true"></SharePoint:ScriptLink>
    
    <SharePoint:CssRegistration  runat="server"  Name=" http://msk-sp-app-t2/_layouts/15/style/voteCss.css" After="corev4.css"></SharePoint:CssRegistration>
  
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:Label ID="tnh" runat="server" ClientIDMode="Static"></asp:Label>
    
    <div id="allInfo">
        <div id="users">
            <asp:Panel runat="server" ID="customTablePanel"></asp:Panel>
        </div>
        
        <asp:Label runat="server" ID="countUser"></asp:Label>
        <div id="blockComment">
            <asp:Label runat="server" Text="Укажите почему Вы выбрали именно этого сотрудника" ID="reason"></asp:Label>
            <br />
            <br />
            <asp:TextBox runat="server" ID="commentTextBoxId" ClientIDMode="Static" Text="" Width="450px" Height="80px" TextMode="MultiLine"></asp:TextBox>
            <br />
            <br />
            <asp:RequiredFieldValidator runat="server" ID="range" ControlToValidate="commentTextBoxId" Text="Обязательное поле" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <%--<input type="button" value="Отправить" id="inputButton" onclick="SendInputButton()" title="Отправить" />--%>
            <asp:Button runat="server" OnClick="InsertMessageInList" Text="Отправить" ID="sendCommentID"   />
            <input type="hidden" runat="server" id="userFIO" clientidmode="Static"/>
            <input type="hidden" runat="server" id="userDepartment" clientidmode="Static"/>
            <input type="hidden" runat="server" id="userComment" clientidmode="Static" />
            <input type="hidden" runat="server" id="userEmail" clientidmode="Static" />            
           
            <br />
            <br />  
            <asp:Label runat="server" ID="error" ForeColor="Red" Font-Size="16px"></asp:Label>
        </div>
    </div>
    
        
 
   
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Vote Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >

</asp:Content>
