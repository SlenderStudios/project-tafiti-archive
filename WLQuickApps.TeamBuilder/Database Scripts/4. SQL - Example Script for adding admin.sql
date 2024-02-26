-- get the userid
declare @UserID uniqueidentifier
select top 1 @UserID = UserID from aspnet_users

-- get the application id
declare @ApplicationID uniqueidentifier
select top 1 @ApplicationID = ApplicationID from aspnet_Applications

-- insert the role
insert into aspnet_Roles(ApplicationId, RoleId, RoleName, LoweredRoleName, Description) values(@ApplicationID, '076f7301-8170-418a-9f6b-e9ab4bff833d', 'Admin', 'admin', '')

-- Get the user ID of the first user in the database
insert into aspnet_UsersInRoles(userID, roleid) values(@UserID, '076f7301-8170-418a-9f6b-e9ab4bff833d')