using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Collections;
using Microsoft.Office.Server.UserProfiles;


namespace VoteForUsers.Layouts.VoteForUsers
{
    public partial class VoteUsers : LayoutsPageBase
    {
        private List<UserInfo> userInf = new List<UserInfo>();

        public List<UserInfo> UserInf
        {
            get
            {
                return userInf;
            }
        }

        public Dictionary<string, string> EmailVsDepartment = new Dictionary<string, string>();        

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack || userFIO.Value == "")
            {
                SPWeb web = SPContext.Current.Web;
                //SPUserCollection users = web.SiteUsers;        
                SPList listCheck = web.Lists["BestEmpReport"];
                SPListItemCollection collItemCheck = listCheck.GetItems("Title"); //можно указать через запятую


                bool ifUserNotAnswerBool = true;
                foreach (SPListItem listItem in collItemCheck)
                {
                    if (listItem["Title"].ToString() == web.CurrentUser.Name)
                    {
                        ifUserNotAnswerBool = false;
                        IfUserAnswer();
                        break;
                    }

                }

                if (ifUserNotAnswerBool)
                {
                    IfUserNotAnswer(web);
                }

            }
            
            
            
                
            
        }
        public void IfUserNotAnswer(SPWeb web)
        {
            SPList list = web.Lists["BestEmpDepartment"];
            SPListItemCollection collItem = list.GetItems("ListEmail","Score"); //можно указать через запятую

            
            GetDepartmentFromList(web);

            
            foreach (SPListItem listItem in collItem)
            {
                
                var mails = listItem["ListEmail"].ToString().Split(';');
                var usersFromMails = mails.Select(x => x.Split('@')[0]).ToArray();
                int score = Convert.ToInt32(listItem["Score"]);

               

                for (int i = 0; i < mails.Length; i++)
                {
                    string linkToPhotoUsers = web.Url + "/ImagesLibrary/" + usersFromMails[i] + "_medium.jpeg ";
                    try
                    {
                        SPUser user = web.SiteUsers.GetByEmail(mails[i]);
                        int sunbtringStartIndex = user.Name.IndexOf('(', 0) + 1;
                        int substringEndIndex = user.Name.Length - 1;
                        int length = substringEndIndex - sunbtringStartIndex;

                        UserInf.Add(new UserInfo
                        {
                            userLogin = usersFromMails[i],
                            userUrl = linkToPhotoUsers,
                            userDisplayNameFull = user.Name.Substring(sunbtringStartIndex, length),
                            email = mails[i],
                            score = score
                        });
                    }
                    catch (Exception)
                    {
                        UserInf.Add(new UserInfo
                        {
                            userLogin = usersFromMails[i],
                            userUrl = linkToPhotoUsers,
                            userDisplayNameFull = "Нет информации",
                            email = mails[i],
                            score = score
                        });
                    }

                }

            }

            GenerateTable(web);
        }
        public void IfUserAnswer()
        {
            commentTextBoxId.Text = "";
            sendCommentID.Visible = false;
            commentTextBoxId.Visible = false;
            reason.Text = "";
            tnh.Style.Add("color","green");
            tnh.Style.Add("font-size", "20px");
            tnh.Text = "Вы уже проголосовали";

        }
        public void GetDepartmentFromList(SPWeb web)
        {
            SPList list = web.Lists["BestEmpDepartment"];
            SPListItemCollection collItem = list.GetItems("ListEmail", "Department"); //можно указать через запятую

            foreach (SPListItem item in collItem)
            {
                EmailVsDepartment.Add(item["ListEmail"].ToString(), item["Department"].ToString());
            }
        }
        public void GenerateTable(SPWeb web)
        {
            
            HtmlTable table1 = new HtmlTable();
            table1.ID = "userTable";
           

            HtmlTableRow row;
            HtmlTableCell cell;
           
            int cellInRowCount = 5;

            int temp_crc = 0;
            row = new HtmlTableRow();

            int buildTotal = 0;
            float SumAllScore = 0;
            foreach (var sum in UserInf)
            {
                SumAllScore += sum.score;
            }

            foreach (var department in EmailVsDepartment)
            {
                temp_crc++;
                cell = new HtmlTableCell();
                SPFile file = web.GetFile($"/UserImagesLibrary/{UserInf[buildTotal].userLogin}_medium.jpeg ");
                cell.InnerHtml = $@"<div id = '{UserInf[buildTotal].userLogin}'style='height: 250px;position: relative; margin-top: 25px;' 
                onclick = 'sendCommentBestEmp(""{UserInf[buildTotal].userLogin}"")'>";
                
                if (file.Exists && UserInf[buildTotal].email == department.Key)
                {
                    cell.InnerHtml += $"<img src = '{UserInf[buildTotal].userUrl}' alt = ''/>";
                }              
                else
                {
                    cell.InnerHtml += $"<img src = '{web.Url}/_layouts/15/images/photos/nofotoemploeem-small.png' alt = ''/>";
                }
                cell.InnerHtml += $"<p class='displayName'>{UserInf[buildTotal].userDisplayNameFull}</p>";
                cell.InnerHtml += $"<p class='department'>{department.Value}</p>";
                cell.InnerHtml += $"<div class='myProgress'><div id='progressBar{UserInf[buildTotal].userLogin}' style='bottom:0; position: absolute;'></div></div>";
                cell.InnerHtml += $"<div id='percent-{UserInf[buildTotal].userLogin}'>{Convert.ToInt32((UserInf[buildTotal].score / SumAllScore) * 100)}</div>";
                cell.InnerHtml += $"<input type='hidden' class='userEmailGenTable' value='{UserInf[buildTotal].email}' />";
                cell.InnerHtml += "</div>";


                row.Cells.Add(cell);
                buildTotal++;
                if (temp_crc == cellInRowCount || buildTotal == UserInf.Count)
                {

                    table1.Rows.Add(row);
                    row = new HtmlTableRow();
                    temp_crc = 0;
                }

            }

            row = null;
            

            customTablePanel.Controls.Add(table1);
            
        }
        public void  InsertMessageInList(Object sender,EventArgs e)
        {


            SPWeb currentWeb = SPContext.Current.Web;
            SPListItemCollection listItems = currentWeb.Lists["BestEmpReport"].Items;          
            SPListItem listitem = listItems.Add();

           
            try
            {
                if (userFIO.Value != "")
                {
                    listitem["Title"] = currentWeb.CurrentUser.Name;
                    listitem["Name"] = userFIO.Value;
                    listitem["DepartmentReport"] = userDepartment.Value;
                    listitem["CommentReport"] = commentTextBoxId.Text;
                    listitem["userEmail"] = userEmail.Value;                    
                    listitem.Update();

                    reason.Text = "";
                    commentTextBoxId.Text = "";
                    sendCommentID.Visible = false;
                    commentTextBoxId.Visible = false;
                    tnh.Style.Add("color", "green");
                    tnh.Style.Add("font-size", "20px");
                    tnh.Text = "Спасибо за Ваш голос";
                    error.Text = "";
                }
                else
                {
                    reason.Text = "";
                    error.Text = "Необходимо выбрать сотрудника";
                }
                
            }
            catch (Exception ex)
            {

                error.Text = ex.Message;
            }           
                        
        }

    }
}
