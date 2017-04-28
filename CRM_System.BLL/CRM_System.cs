 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM_System.Model;
using CRM_System.DAL;
//using CRM_System.Linq;

namespace CRM_System.BLL
{
	
	public partial class Mpr_Admin_ButtonRoleService:SysBllBase<Mpr_Admin_ButtonRole>
    { 
		public override RepositoryBase<Mpr_Admin_ButtonRole> repository {set;get;}
		public Mpr_Admin_ButtonRoleService(Mpr_Admin_ButtonRoleRepository repository)
        {
            this.repository = repository;
        }
        public Mpr_Admin_ButtonRoleService()
        {
            this.repository = new Mpr_Admin_ButtonRoleRepository();
        }
    }
	
	public partial class Mpr_Admin_MenuService:SysBllBase<Mpr_Admin_Menu>
    { 
		public override RepositoryBase<Mpr_Admin_Menu> repository {set;get;}
		public Mpr_Admin_MenuService(Mpr_Admin_MenuRepository repository)
        {
            this.repository = repository;
        }
        public Mpr_Admin_MenuService()
        {
            this.repository = new Mpr_Admin_MenuRepository();
        }
    }
	
	public partial class Mpr_Admin_RoleService:SysBllBase<Mpr_Admin_Role>
    { 
		public override RepositoryBase<Mpr_Admin_Role> repository {set;get;}
		public Mpr_Admin_RoleService(Mpr_Admin_RoleRepository repository)
        {
            this.repository = repository;
        }
        public Mpr_Admin_RoleService()
        {
            this.repository = new Mpr_Admin_RoleRepository();
        }
    }
	
	public partial class Mpr_Basic_ParamsService:SysBllBase<Mpr_Basic_Params>
    { 
		public override RepositoryBase<Mpr_Basic_Params> repository {set;get;}
		public Mpr_Basic_ParamsService(Mpr_Basic_ParamsRepository repository)
        {
            this.repository = repository;
        }
        public Mpr_Basic_ParamsService()
        {
            this.repository = new Mpr_Basic_ParamsRepository();
        }
    }
	
	public partial class Mpr_CustomerService:SysBllBase<Mpr_Customer>
    { 
		public override RepositoryBase<Mpr_Customer> repository {set;get;}
		public Mpr_CustomerService(Mpr_CustomerRepository repository)
        {
            this.repository = repository;
        }
        public Mpr_CustomerService()
        {
            this.repository = new Mpr_CustomerRepository();
        }
    }
	
	public partial class Mpr_Customer_LogService:SysBllBase<Mpr_Customer_Log>
    { 
		public override RepositoryBase<Mpr_Customer_Log> repository {set;get;}
		public Mpr_Customer_LogService(Mpr_Customer_LogRepository repository)
        {
            this.repository = repository;
        }
        public Mpr_Customer_LogService()
        {
            this.repository = new Mpr_Customer_LogRepository();
        }
    }
	
	public partial class Mpr_InterfaceCall_LogService:SysBllBase<Mpr_InterfaceCall_Log>
    { 
		public override RepositoryBase<Mpr_InterfaceCall_Log> repository {set;get;}
		public Mpr_InterfaceCall_LogService(Mpr_InterfaceCall_LogRepository repository)
        {
            this.repository = repository;
        }
        public Mpr_InterfaceCall_LogService()
        {
            this.repository = new Mpr_InterfaceCall_LogRepository();
        }
    }
	
	public partial class Mpr_MaterialService:SysBllBase<Mpr_Material>
    { 
		public override RepositoryBase<Mpr_Material> repository {set;get;}
		public Mpr_MaterialService(Mpr_MaterialRepository repository)
        {
            this.repository = repository;
        }
        public Mpr_MaterialService()
        {
            this.repository = new Mpr_MaterialRepository();
        }
    }
	
	public partial class Mpr_Material_TypeService:SysBllBase<Mpr_Material_Type>
    { 
		public override RepositoryBase<Mpr_Material_Type> repository {set;get;}
		public Mpr_Material_TypeService(Mpr_Material_TypeRepository repository)
        {
            this.repository = repository;
        }
        public Mpr_Material_TypeService()
        {
            this.repository = new Mpr_Material_TypeRepository();
        }
    }
	
	public partial class Mpr_OrganizationService:SysBllBase<Mpr_Organization>
    { 
		public override RepositoryBase<Mpr_Organization> repository {set;get;}
		public Mpr_OrganizationService(Mpr_OrganizationRepository repository)
        {
            this.repository = repository;
        }
        public Mpr_OrganizationService()
        {
            this.repository = new Mpr_OrganizationRepository();
        }
    }
	
	public partial class Sys_AdminService:SysBllBase<Sys_Admin>
    { 
		public override RepositoryBase<Sys_Admin> repository {set;get;}
		public Sys_AdminService(Sys_AdminRepository repository)
        {
            this.repository = repository;
        }
        public Sys_AdminService()
        {
            this.repository = new Sys_AdminRepository();
        }
    }
	
	public partial class Sys_Error_logService:SysBllBase<Sys_Error_log>
    { 
		public override RepositoryBase<Sys_Error_log> repository {set;get;}
		public Sys_Error_logService(Sys_Error_logRepository repository)
        {
            this.repository = repository;
        }
        public Sys_Error_logService()
        {
            this.repository = new Sys_Error_logRepository();
        }
    }
	
	public partial class Sys_RoleService:SysBllBase<Sys_Role>
    { 
		public override RepositoryBase<Sys_Role> repository {set;get;}
		public Sys_RoleService(Sys_RoleRepository repository)
        {
            this.repository = repository;
        }
        public Sys_RoleService()
        {
            this.repository = new Sys_RoleRepository();
        }
    }
	
	public partial class View_Phone_WareHouseService:SysBllBase<View_Phone_WareHouse>
    { 
		public override RepositoryBase<View_Phone_WareHouse> repository {set;get;}
		public View_Phone_WareHouseService(View_Phone_WareHouseRepository repository)
        {
            this.repository = repository;
        }
        public View_Phone_WareHouseService()
        {
            this.repository = new View_Phone_WareHouseRepository();
        }
    }
	
	public partial class View_User_PhoneService:SysBllBase<View_User_Phone>
    { 
		public override RepositoryBase<View_User_Phone> repository {set;get;}
		public View_User_PhoneService(View_User_PhoneRepository repository)
        {
            this.repository = repository;
        }
        public View_User_PhoneService()
        {
            this.repository = new View_User_PhoneRepository();
        }
    }
	
}