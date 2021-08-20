# Rest API with .net and Entity Framework
This project consists of a simple product register that will be used to enter all the
our items in the solution.

# Demands
To meet all requirements, this project implements two demands that are
specified below

# First demand:

- The user must be able to register new products in an end-point available in the REST API.

- Each product has a description, a cost price and a category that are entered by the user in the
time of registration.

- In addition to the three basic fields described above, each new product registered must record the creation date, an ID that should be automatically generated and a sales price that should be calculated using a query to the service built in the [restApi01](https://github.com/ricardo-lampert/restApi01) project.

- Also, every time a new product is
registered and in its description the word "Softplayer", this item should be automatically
framed in the "Softplan" category.


# Second demand:

- The user must be able to consult the products registered through an end-point available in our
REST API



# Endpoints
Two entries are available:
- **api01/** = Lists all registered products
- **api01/insert** =  Receive a product object (JSON) and insert on database.
    

# Use cases



1. **Normal insertion** must **register**
2. Insertion with **Softplayer in the description**, must **change the category** to softplan and **register**
3. Insertion with **Softplayer in the description**, must **insert the category** for softplan and **register**
4. **Normal insertion** must **register** (to check the display order later)
5. Insert **without description**, must return an **error** 
6. Insert with **description greater than limit**, must return an **error**
7. **Uncategorized input**, should return an **error**
8. Entry with **cost 0**, should return an **error**
9. Entry with **negative cost**, should return an **error**

# Makefile

**make run** to run the aplication

**make test** to ran the tests for the aplication 
