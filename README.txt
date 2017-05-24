# Warehouse description

This application is solution to the task given by Yara Bulgaria ltd.

Warehouse application has the following funcionality:

	> users in role "Admin":
	  - add silo/tank
	  - edit/delete silo/tank
	  
	> all registered users:
	  - view all silos/tanks
	  - add commodity/fertilizer to silo/tank
	  - remove commodity/fertilizer from silo/tank
	  - table view of all operations for each silo/tank
	  

Classes and methods description:
	> silo.cs:
		properties:
		  - Id 
		  - Name
		  - CurrentCommodity
		  - CurrentLoad
		  - MaxCapacity
		  - SiloNumber
		  - Operations
		  - SiloCreatorId
		 
		methods:
		  - AddCommodity: 
		    * reqiures 2 parameters (Commodity commodity, string name)
			* validates:
			  . if the user tries to add commodity which exeeds the silo capacity the methods returns exception
			  . if the commodity amount <= 0 the methods returns exception
			  . if the commodity name is different from the current commodity in the silo, the methods returns exception
			  . if the silo is empty - current commodity == commodity name
			  
			* saves the operation in the database
			
		  - ExportCommodity:
		    * reqiures 2 parameters (Commodity commodity, string name)
			* validates:
			  . if the user tries to export more than current load level - the method returns exception
			  . if the user tries to export negative amount of the commodity - the method returns exception
			  . if the user tries to export different commodity from the current commodity in the silo - the method returns exception
			  . if current load level after the operation equels 0, the silo is marked as empty
			  
			* saves the operation in the database
			
		  - CanDeleteSilo:
			* reqiures 1 parameter (DeleteSiloModel silo)
			* validates:
			  . if there is commodity in the silo - the silo cannot be deleted
			  . if the silo is empty - it can be deleted
			  
			  
	> tank.cs:
		properties:
		  - Id 
		  - Name
		  - CurrentFertilizer
		  - CurrentLoad
		  - MaxCapacity
		  - Number
		  - Operations
		  - TankCreatorId
		 
		methods:
		  - AddFertilizer: 
		    * reqiures 2 parameters (Fertilizer fertilizer, string name)
			* validates:
			  . if the user tries to add fertilizer which exeeds the tank capacity the methods returns exception
			  . if the fertilizer amount <= 0 the methods returns exception
			  . if the fertilizer name is different from the current fertilizer in the tank, the methods returns exception
			  . if the tank is empty - current fertilizer == fertilizer name
			  
			* saves the operation in the database
			
		  - ExportFertilizer:
		    * reqiures 2 parameters (Fertilizer fertilizer, string name)
			* validates:
			  . if the user tries to export more than current load level - the method returns exception
			  . if the user tries to export negative amount of the fertilizer - the method returns exception
			  . if the user tries to export different fertilizer from the current fertilizer in the tank - the method returns exception
			  . if current load level after the operation equels 0, the tank is marked as empty
			  
			* saves the operation in the database
			
		  - CanDelete:
			* reqiures 1 parameter (DeleteTankModel tank)
			* validates:
			  . if there is fertilizer in the tank - the tank cannot be deleted
			  . if the tank is empty - it can be deleted
			  
			  
	> operation.cs / TankOperation.cs:
	    properties:
		  - Id 
          - ActionDate
          - AmountBeforeAction 
          - OperationName 
          - ActionAmount
          - AmountAfterAction 
          - SiloId/TankId
          - OperatorName
          - CommodityName/Fertilizerertilizer 
	
	
	> Commodity.cs / Fertilizer.cs:
		properties:
		  - Name
		  - Amount

	  