using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// This class is POCO of Tasks. This was created to hold the data as a DB modal
/// Create By Indika Maligaspe
/// Created on 03/03/2014    
/// </summary>
public class Task 
{
    public Task(string Name,string projectName,DateTime PlannedDate,string ScheduledTime,
                string DurationDisplay, string Leverage, string Reason, string Priority,int userID) 
    {
        this.Name=Name;
        this.projectName = projectName;
        this.PlannedDate=PlannedDate;
        this.ScheduledTime = ScheduledTime;
        this.DurationDisplay = DurationDisplay;
        this.Leverage = Leverage;
        this.Reason = Reason;
        this.Priority = Priority;
        this.userID = userID;
    }
    
    public Task(){
        
    }

   
    public string Name {get;set;}
    public int ID {get;set;}
    public int ProjectID {get;set;}
    public string projectName{get;set;}
    public DateTime PlannedDate {get;set;}
    public string ScheduledTime {get;set;}
    public int Duration {get;set;}
    public string DurationMessure {get;set;}
    public string DurationDisplay {get;set;}
    public string Leverage {get;set;}
    public string Reason {get;set;}
    public string Priority {get;set;}
    public int priorityID {get;set;}
    public int userID{get;set;}

}
