using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Project
/// </summary>
public class Project
{
    public Project()
    {
        
    }
    
    public Project(int ProjectID,string ProjectName)
    {
        this.ProjectID = ProjectID;
        this.ProjectName = ProjectName;
    }

    public int ProjectID{get;set;}
    public string ProjectName {get;set;}
}
