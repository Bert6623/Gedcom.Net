#region using references
using System;
using sp_wsatTableAdapters;//import wsat table adapters
#endregion

/// <summary>
/// This is a class file with methods that allow the DataSet to work 
/// with the stored procedures for efficient paging.
/// </summary>
[System.ComponentModel.DataObject]
public class sp_wsat_Users
{
    #region declare table adapter

    //---------------------------------------------------------------
    // we declare the table adapter object first (sp_wsat.xsd)
    //---------------------------------------------------------------
    private sp_wsat_UsersTableAdapter sp_wsatUsersAdapter = null;
    protected sp_wsat_UsersTableAdapter Adapter
    {
        get
        {
            if (sp_wsatUsersAdapter == null)
                sp_wsatUsersAdapter = new sp_wsat_UsersTableAdapter();

            return sp_wsatUsersAdapter;
        }
    }

    #endregion

    #region count results

    //---------------------------------------------------------------
    // get total number of users from dataset
    //---------------------------------------------------------------
    public int TotalNumberOfUsersByName(string UserName) // by NAME
    {
        return Convert.ToInt32(Adapter.TotalNumberOfUsersByName(UserName));
    }

    public int TotalNumberOfUsersByIsLockedOut() // by ISLOCKEDOUT
    {
        return Convert.ToInt32(Adapter.TotalNumberOfUsersByIsLockedOut());
    }

    public int TotalNumberOfUsersByRole(string RoleName) // by ROLE
    {
        return Convert.ToInt32(Adapter.TotalNumberOfUsersByRole(RoleName));
    }

    public int TotalNumberOfUsersByIsApproved() // by ISAPPROVED
    {
        return Convert.ToInt32(Adapter.TotalNumberOfUsersByIsApproved());
    }

    public int TotalNumberOfUsersBySearch(string Email, string UserName) // by SEARCH
    {
        return Convert.ToInt32(Adapter.TotalNumberOfUsersBySearch(Email, UserName));
    }

    #endregion

    #region return adapters

    //---------------------------------------------------------------
    // get users by NAME (WITH sorting) from dataset
    //---------------------------------------------------------------
    [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
    public sp_wsat.sp_wsat_UsersDataTable sp_wsat_GetUsersByName(string UserName, int startRowIndex, int maximumRows)
    {
        return Adapter.sp_wsat_GetUsersByName(UserName, startRowIndex, maximumRows);
    }

    //---------------------------------------------------------------
    // get users by ISLOCKEDOUT (WITH sorting) from dataset
    //---------------------------------------------------------------
    public sp_wsat.sp_wsat_UsersDataTable sp_wsat_GetUsersByIsLockedOut(int startRowIndex, int maximumRows)
    {
        return Adapter.sp_wsat_GetUsersByIsLockedOut(startRowIndex, maximumRows);
    }

    //---------------------------------------------------------------
    // get users by ROLENAME (WITH sorting) from dataset
    //---------------------------------------------------------------
    public sp_wsat.sp_wsat_UsersDataTable sp_wsat_GetUsersByRole(string RoleName, int startRowIndex, int maximumRows)
    {
        return Adapter.sp_wsat_GetUsersByRole(RoleName, startRowIndex, maximumRows);
    }

    //---------------------------------------------------------------
    // get users by ISAPPROVED (WITH sorting) from dataset
    //---------------------------------------------------------------
    public sp_wsat.sp_wsat_UsersDataTable sp_wsat_GetUsersByIsApproved(int startRowIndex, int maximumRows)
    {
        return Adapter.sp_wsat_GetUsersByIsApproved(startRowIndex, maximumRows);
    }

    //---------------------------------------------------------------
    // get users by SEARCH (WITH sorting) from dataset
    //---------------------------------------------------------------
    public sp_wsat.sp_wsat_UsersDataTable sp_wsat_GetUsersBySearch(string Email, string UserName, int startRowIndex, int maximumRows)
    {
        return Adapter.sp_wsat_GetUsersBySearch(Email, UserName, startRowIndex, maximumRows);
    }

    #endregion
}