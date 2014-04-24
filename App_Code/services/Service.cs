using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using WebMatrix.Data;
using log4net;

/// <summary>
/// Summary description for Service
/// </summary>
public class Service
{
    log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Service));
    
    string SQL = null;
    private Database _db;
    public Service()
    {
       
    }
    private void GetConnection(){
        try{
           _db =  Database.Open("Salasma");
        }catch(Exception){
            
        }
        
    }
    private void FreeConnection(){
        try{
            _db.Close();
        }catch(Exception){
            
        }
        
    }

    public List<Task> GetTodaysTasks(){
            SQL =   " select a.ID,a.Name 'Name' ,b.ProjectName 'ProjectName',PlannedDate,a.ScheduledTime,a.Duration, "+
                " a.DurationMessure,a.DurationDisplay,a.Leverage,a.Reason,a.Priority,a.userID "+
                " from Task a , Project b , userprojects c "+
                " where Date(PlannedDate)= curdate() "+
                " and c.ID = a.UserProjectID "+
                " and b.ProjectID = c.ProjectID ";

                return GetTaskList(SQL);
        
    }
   
    public List<Task> GetOverDueTasks(){
            SQL =   " select a.ID,a.Name 'Name' ,b.ProjectName 'ProjectName',PlannedDate,a.ScheduledTime,a.Duration, "+
                    " a.DurationMessure,a.DurationDisplay,a.Leverage,a.Reason,a.Priority,a.userID "+
                    " from Task a , Project b , userprojects c "+
                    " where Date(PlannedDate) < curdate() "+
                    " and c.ID = a.UserProjectID "+
                    " and b.ProjectID = c.ProjectID ";

            return GetTaskList(SQL);
    }
   
    public List<Task> GetTasksForTomorrow(){
            SQL =   " select a.ID,a.Name 'Name' ,b.ProjectName 'ProjectName',PlannedDate,a.ScheduledTime,a.Duration, "+
                    " a.DurationMessure,a.DurationDisplay,a.Leverage,a.Reason,a.Priority,a.userID "+
                    " from Task a , Project b , userprojects c "+
                    " where Date(PlannedDate)  =  curdate() +1"+
                    " and c.ID = a.UserProjectID "+
                    " and b.ProjectID = c.ProjectID ";

           return GetTaskList(SQL);
    }
   
    public List<Task> GetTasksForDayAfter(){
      
            SQL =   " select a.ID,a.Name 'Name' ,b.ProjectName 'ProjectName',PlannedDate,a.ScheduledTime,a.Duration, "+
                    " a.DurationMessure,a.DurationDisplay,a.Leverage,a.Reason,a.Priority,a.userID "+
                    " from Task a , Project b , userprojects c "+
                    " where Date(PlannedDate) =  curdate()+2 "+
                    " and c.ID = a.UserProjectID "+
                    " and b.ProjectID = c.ProjectID ";

            return GetTaskList(SQL);
    } 
    
    public List<Task> GetTasksForTwoDaysAfter(){
    
            SQL =   " select a.ID,a.Name 'Name' ,b.ProjectName 'ProjectName',PlannedDate,a.ScheduledTime,a.Duration, "+
                    " a.DurationMessure,a.DurationDisplay,a.Leverage,a.Reason,a.Priority,a.userID "+
                    " from Task a , Project b , userprojects c "+
                    " where Date(PlannedDate) =  curdate()+3 "+
                    " and c.ID = a.UserProjectID "+
                    " and b.ProjectID = c.ProjectID ";
           return GetTaskList(SQL);
    }

    public List<Task> GetTasksForSpecificDate(String date){
        GetConnection();
        List<Task> TaskList = new List<Task>();
        try{
            SQL =   " select a.ID,a.Name 'Name' ,b.ProjectName 'ProjectName',PlannedDate,a.ScheduledTime,a.Duration, "+
                    " a.DurationMessure,a.DurationDisplay,a.Leverage,a.Reason,a.Priority,a.userID "+
                    " from Task a , Project b , userprojects c "+
                    " where Date(PlannedDate) =  Date('"+date+"') "+
                    " and c.ID = a.UserProjectID "+
                    " and b.ProjectID = c.ProjectID ";
           logger.Debug(SQL);
           var Results = _db.Query(SQL);

           foreach(var result in Results){
               Task Task = new Task(result.ID,result.Name,result.ProjectName,result.PlannedDate,result.ScheduledTime,
                                    result.DurationDisplay,result.Leverage,result.Reason,result.Priority,result.userID);
               TaskList.Add(Task);
           }
      }catch(Exception e){
           FreeConnection(); 
        }
        
        finally{
            FreeConnection();
        }
       return(TaskList);
    }

    
    public List<Task> GetAllUnplannedTasks(){
            SQL =   " select a.ID,a.Name 'Name' ,b.ProjectName 'ProjectName',PlannedDate,a.ScheduledTime,a.Duration, "+
                    " a.DurationMessure,a.DurationDisplay,a.Leverage,a.Reason,a.Priority,a.userID "+
                    " from Task a , Project b , userprojects c "+
                    " where Date(PlannedDate) >  curdate() "+
                    " and ScheduledTime='00:00-00:00'"+
                    " and c.ID = a.UserProjectID "+
                    " and b.ProjectID = c.ProjectID ";
 
          return GetTaskList(SQL);
    }

    public List<Task> GetTaskList(string SQL){
        List<Task> TaskList = new List<Task>();
        logger.Debug(SQL);
        try{
            GetConnection();
            var Results = _db.Query(SQL);
            foreach(var result in Results){
               Task Task = new Task(result.ID,result.Name,result.ProjectName,result.PlannedDate,result.ScheduledTime,
                                    result.DurationDisplay,result.Leverage,result.Reason,result.Priority,result.userID);
               TaskList.Add(Task);
           }    
        }catch(Exception e){
           FreeConnection(); 
        }
        return TaskList;
    }
    public List<Project> GetProjectList(){
        GetConnection();
        List<Project> ProjectList = new List<Project>();
        try{
           SQL =   "select ProjectID,ProjectName from project";
           var Results = _db.Query(SQL);
           foreach(var result in Results){
               Project project = new Project(result.ProjectID,result.ProjectName);
               ProjectList.Add(project);
           }
        }catch(Exception e){
           FreeConnection(); 
        }
        finally{
            FreeConnection();
        }
       return(ProjectList);
    }
    
    public int SaveTask(Task task){
        int id = -1;
        try{
            SQL =   "Insert into Task (`Name`,`UserProjectID`,`PlannedDate`,`ScheduledTime`,`Duration`,`DurationMessure`, "+
                    "`DurationDisplay`,`Leverage`,`Reason`,`Priority`,`userID`) "+
                    "values "+
                    "('"+task.Name.Replace("'","\\'").Replace('"','\"')+
                    "','"+GetUserProject(task.userID,task.ProjectID).ID+
                    " ','"+String.Format("{0:yyyy-MM-dd 00:00:00}",task.PlannedDate)+
                    " ','"+task.ScheduledTime+
                    " ','"+task.Duration+
                    " ','"+task.DurationMessure+
                    "','"+task.DurationDisplay+
                    " ','"+task.Leverage+
                    " ','"+task.Reason.Replace("'","\\'").Replace('"','\"')+
                    " ','"+task.Priority+
                    " ','"+task.userID+"') ";

           logger.Debug(SQL);
           id = _db.Execute(SQL);
           logger.Debug(id);

        }catch(Exception e){
            logger.Debug("Test data..."+e.Data);
            FreeConnection(); 
        }
        finally{
            FreeConnection();
        }
        return id;
    }

    public int UpdateTask(Task task){
        int id = -1;
        try{
            SQL =   "Update  Task set  "+
                    " `Name` = ('"+task.Name.Replace("'","\\'").Replace('"','\"')+
                    "',`UserProjectID` = '"+GetUserProject(task.userID,task.ProjectID).ID+
                    " ',`PlannedDate` = '"+String.Format("{0:yyyy-MM-dd 00:00:00}",task.PlannedDate)+
                    " ',`ScheduledTime` = '"+task.ScheduledTime+
                    " ',`Duration` = '"+task.Duration+
                    " ',`DurationMessure` = '"+task.DurationMessure+
                    "',`DurationDisplay` = '"+task.DurationDisplay+
                    " ',`Leverage` = '"+task.Leverage+
                    " ',`Reason` = '"+task.Reason.Replace("'","\\'").Replace('"','\"')+
                    " ',`Priority` = '"+task.Priority+
                    " ',`userID` = '"+task.userID+"' where id=`"+task.ID+"`";

           logger.Debug(SQL);
           id = _db.Execute(SQL);
           logger.Debug(id);

        }catch(Exception e){
            logger.Debug("Test data..."+e.Data);
            FreeConnection(); 
        }
        finally{
            FreeConnection();
        }
        return id;
    }
    public UserProjects GetUserProject(int userID,int projectID){
        UserProjects userProject = null;
        List<Project> ProjectList = new List<Project>();
        SQL = "select ID,UserID,ProjectID from userprojects "+
              "where UserID='"+userID+"' and ProjectID='"+projectID+"'";
        try{
          GetConnection();  
          var Results = _db.Query(SQL);
          foreach(var result in Results){
               userProject = new UserProjects(result.ID,result.UserID,result.ProjectID);
           }

        }catch(Exception err){
            FreeConnection();
        }
        finally{
            FreeConnection();
        }
        return userProject;
    }
}
