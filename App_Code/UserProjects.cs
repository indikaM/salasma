using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for UserProjects
/// </summary>
public class UserProjects
{
    public UserProjects()
    {
        
    }

    public UserProjects(int ID,int UserID,int ProjectID)
    {
        this.ID = ID;
        this.UserID = UserID;
        this.ProjectID = ProjectID;
    }

    public int ID{get;set;}
    public int UserID{get;set;}
    public int ProjectID{get;set;}

}
