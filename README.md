
# Selling Book Project (1670 Project)

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Introduction

The Selling Book Project is a web application built using the .NET MVC framework. It allows users to browse, search, and purchase books online. The platform provides an intuitive interface for both customers and administrators to manage book listings, orders, and user accounts.

## Features

- **User Authentication:** Secure registration and login system.
- **Book Catalog:** Browse and search through a wide range of books.
- **Shopping Cart:** Add books to the cart and proceed to checkout.
- **Order Management:** Users can view their order history.
- **Admin Panel:** Manage books, categories, and orders.
- **Responsive Design:** Optimized for both desktop and mobile devices.

## Technologies Used

- **Framework:** ASP.NET MVC
- **Language:** C#
- **Database:** SQL Server
- **ORM:** Entity Framework
- **Frontend:** HTML5, CSS3, JavaScript, jQuery
- **Styling:** Bootstrap

## Getting Started

### Prerequisites

Before you begin, ensure you have met the following requirements:

- Visual Studio 2019 or later
- .NET Framework 4.7.2 or later
- SQL Server 2016 or later

### Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/selling-book-project.git
   ```

2. **Open the Solution**

   Navigate to the cloned directory and open the `SellingBookProject.sln` file with Visual Studio.

3. **Restore NuGet Packages**

   Visual Studio will automatically restore the required NuGet packages. If not, right-click on the solution and select `Restore NuGet Packages`.

4. **Update Database Connection**

   Update the database connection string in the `web.config` file to point to your SQL Server instance.

   ```xml
   <connectionStrings>
       <add name="DefaultConnection" connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=SellingBookDB;Integrated Security=True" providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

5. **Apply Migrations**

   Open the Package Manager Console and run:

   ```bash
   Update-Database
   ```

6. **Run the Application**

   Press `F5` or click the `Start` button in Visual Studio to run the application.

## Usage

- **Browsing Books:** Navigate through different categories or use the search bar to find specific books.
- **Purchasing Books:** Add desired books to your cart and proceed to checkout.
- **Admin Operations:** Log in as an admin to add, update, or remove books and manage orders.


## Contact

**Your Name**

- Email: [khanhnoel2003@gmail.com](khanhnoel2003@gmail.com)
- LinkedIn: [Phùng Khánh](https://www.linkedin.com/in/noelisme2003/)
- GitHub: [Phung Khanh](https://github.com/noelisme)
