# HotelBookingSystem
 
Extra features : 
- using SMTP to send emails
- Unit of work
- Generic repository
- N-tier architecture
- Using identity and roles 
- Using Automapper
- force email conformation on register 
- Forget password 

Default admin account "user : Admin , password : 123456" 
you can create your own user account to figure out force email conformation and forgot password features

I didnot use authorize("Admin") attribute on endpoints like getAll() although it is specific for admins to make sure that if you forgot registering using admin account you can also access it 
 
You can find ERD and database backup in the repository "There are also default data on context by seeding" 

All required features are done, if there is anything you want to ask for, contact me..
