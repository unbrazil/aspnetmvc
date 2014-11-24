aspnetmvc
=========

 The common issue when developing controllers interacting with the database is the way that we repeat ourselves to save data. Always using the same logic: 1-Get data from the form; 2-Validate data; 3- Map the data to EF; 4-Pass it to the database context; 5-Persist changes. By thinking about this, this project aims to speed this process by using the abstract controllers provided. So you only need to customize (override methods) that you really need to.
