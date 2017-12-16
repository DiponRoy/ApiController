using System;
using System.Data;
using System.Data.SqlClient;
using V5_RestGetRequest.Data.Models;

namespace V5_RestGetRequest.Data
{
    public class ProductsData
    {

        public static DataTable SelectAll()
        {
            SqlConnection connection = TutorialsteamData.GetConnection();
            string selectStatement
                = "SELECT "  
                + "     [Products].[ProductId] "
                + "    ,[Products].[Name] "
                + "    ,[Products].[Quantity] "
                + "    ,[Products].[BoxSize] "
                + "    ,[Products].[Price] "
                + "FROM " 
                + "     [Products] " 
                + "";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                if (reader.HasRows) {
                    dt.Load(reader); }
                reader.Close();
            }
            catch (SqlException)
            {
                return dt;
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static DataTable Search(string sField, string sCondition, string sValue)
        {
            SqlConnection connection = TutorialsteamData.GetConnection();
            string selectStatement = "";
            if (sCondition == "Contains") {
                selectStatement
                    = "SELECT "
                + "     [Products].[ProductId] "
                + "    ,[Products].[Name] "
                + "    ,[Products].[Quantity] "
                + "    ,[Products].[BoxSize] "
                + "    ,[Products].[Price] "
                + "FROM " 
                + "     [Products] " 
                    + "WHERE " 
                    + "     (@ProductId IS NULL OR @ProductId = '' OR [Products].[ProductId] LIKE '%' + LTRIM(RTRIM(@ProductId)) + '%') " 
                    + "AND   (@Name IS NULL OR @Name = '' OR [Products].[Name] LIKE '%' + LTRIM(RTRIM(@Name)) + '%') " 
                    + "AND   (@Quantity IS NULL OR @Quantity = '' OR [Products].[Quantity] LIKE '%' + LTRIM(RTRIM(@Quantity)) + '%') " 
                    + "AND   (@BoxSize IS NULL OR @BoxSize = '' OR [Products].[BoxSize] LIKE '%' + LTRIM(RTRIM(@BoxSize)) + '%') " 
                    + "AND   (@Price IS NULL OR @Price = '' OR [Products].[Price] LIKE '%' + LTRIM(RTRIM(@Price)) + '%') " 
                    + "";
            } else if (sCondition == "Equals") {
                selectStatement
                    = "SELECT "
                + "     [Products].[ProductId] "
                + "    ,[Products].[Name] "
                + "    ,[Products].[Quantity] "
                + "    ,[Products].[BoxSize] "
                + "    ,[Products].[Price] "
                + "FROM " 
                + "     [Products] " 
                    + "WHERE " 
                    + "     (@ProductId IS NULL OR @ProductId = '' OR [Products].[ProductId] = LTRIM(RTRIM(@ProductId))) " 
                    + "AND   (@Name IS NULL OR @Name = '' OR [Products].[Name] = LTRIM(RTRIM(@Name))) " 
                    + "AND   (@Quantity IS NULL OR @Quantity = '' OR [Products].[Quantity] = LTRIM(RTRIM(@Quantity))) " 
                    + "AND   (@BoxSize IS NULL OR @BoxSize = '' OR [Products].[BoxSize] = LTRIM(RTRIM(@BoxSize))) " 
                    + "AND   (@Price IS NULL OR @Price = '' OR [Products].[Price] = LTRIM(RTRIM(@Price))) " 
                    + "";
            } else if  (sCondition == "Starts with...") {
                selectStatement
                    = "SELECT "
                + "     [Products].[ProductId] "
                + "    ,[Products].[Name] "
                + "    ,[Products].[Quantity] "
                + "    ,[Products].[BoxSize] "
                + "    ,[Products].[Price] "
                + "FROM " 
                + "     [Products] " 
                    + "WHERE " 
                    + "     (@ProductId IS NULL OR @ProductId = '' OR [Products].[ProductId] LIKE LTRIM(RTRIM(@ProductId)) + '%') " 
                    + "AND   (@Name IS NULL OR @Name = '' OR [Products].[Name] LIKE LTRIM(RTRIM(@Name)) + '%') " 
                    + "AND   (@Quantity IS NULL OR @Quantity = '' OR [Products].[Quantity] LIKE LTRIM(RTRIM(@Quantity)) + '%') " 
                    + "AND   (@BoxSize IS NULL OR @BoxSize = '' OR [Products].[BoxSize] LIKE LTRIM(RTRIM(@BoxSize)) + '%') " 
                    + "AND   (@Price IS NULL OR @Price = '' OR [Products].[Price] LIKE LTRIM(RTRIM(@Price)) + '%') " 
                    + "";
            } else if  (sCondition == "More than...") {
                selectStatement
                    = "SELECT "
                + "     [Products].[ProductId] "
                + "    ,[Products].[Name] "
                + "    ,[Products].[Quantity] "
                + "    ,[Products].[BoxSize] "
                + "    ,[Products].[Price] "
                + "FROM " 
                + "     [Products] " 
                    + "WHERE " 
                    + "     (@ProductId IS NULL OR @ProductId = '' OR [Products].[ProductId] > LTRIM(RTRIM(@ProductId))) " 
                    + "AND   (@Name IS NULL OR @Name = '' OR [Products].[Name] > LTRIM(RTRIM(@Name))) " 
                    + "AND   (@Quantity IS NULL OR @Quantity = '' OR [Products].[Quantity] > LTRIM(RTRIM(@Quantity))) " 
                    + "AND   (@BoxSize IS NULL OR @BoxSize = '' OR [Products].[BoxSize] > LTRIM(RTRIM(@BoxSize))) " 
                    + "AND   (@Price IS NULL OR @Price = '' OR [Products].[Price] > LTRIM(RTRIM(@Price))) " 
                    + "";
            } else if  (sCondition == "Less than...") {
                selectStatement
                    = "SELECT " 
                + "     [Products].[ProductId] "
                + "    ,[Products].[Name] "
                + "    ,[Products].[Quantity] "
                + "    ,[Products].[BoxSize] "
                + "    ,[Products].[Price] "
                + "FROM " 
                + "     [Products] " 
                    + "WHERE " 
                    + "     (@ProductId IS NULL OR @ProductId = '' OR [Products].[ProductId] < LTRIM(RTRIM(@ProductId))) " 
                    + "AND   (@Name IS NULL OR @Name = '' OR [Products].[Name] < LTRIM(RTRIM(@Name))) " 
                    + "AND   (@Quantity IS NULL OR @Quantity = '' OR [Products].[Quantity] < LTRIM(RTRIM(@Quantity))) " 
                    + "AND   (@BoxSize IS NULL OR @BoxSize = '' OR [Products].[BoxSize] < LTRIM(RTRIM(@BoxSize))) " 
                    + "AND   (@Price IS NULL OR @Price = '' OR [Products].[Price] < LTRIM(RTRIM(@Price))) " 
                    + "";
            } else if  (sCondition == "Equal or more than...") {
                selectStatement
                    = "SELECT "
                + "     [Products].[ProductId] "
                + "    ,[Products].[Name] "
                + "    ,[Products].[Quantity] "
                + "    ,[Products].[BoxSize] "
                + "    ,[Products].[Price] "
                + "FROM " 
                + "     [Products] " 
                    + "WHERE " 
                    + "     (@ProductId IS NULL OR @ProductId = '' OR [Products].[ProductId] >= LTRIM(RTRIM(@ProductId))) " 
                    + "AND   (@Name IS NULL OR @Name = '' OR [Products].[Name] >= LTRIM(RTRIM(@Name))) " 
                    + "AND   (@Quantity IS NULL OR @Quantity = '' OR [Products].[Quantity] >= LTRIM(RTRIM(@Quantity))) " 
                    + "AND   (@BoxSize IS NULL OR @BoxSize = '' OR [Products].[BoxSize] >= LTRIM(RTRIM(@BoxSize))) " 
                    + "AND   (@Price IS NULL OR @Price = '' OR [Products].[Price] >= LTRIM(RTRIM(@Price))) " 
                    + "";
            } else if (sCondition == "Equal or less than...") {
                selectStatement 
                    = "SELECT "
                + "     [Products].[ProductId] "
                + "    ,[Products].[Name] "
                + "    ,[Products].[Quantity] "
                + "    ,[Products].[BoxSize] "
                + "    ,[Products].[Price] "
                + "FROM " 
                + "     [Products] " 
                    + "WHERE " 
                    + "     (@ProductId IS NULL OR @ProductId = '' OR [Products].[ProductId] <= LTRIM(RTRIM(@ProductId))) " 
                    + "AND   (@Name IS NULL OR @Name = '' OR [Products].[Name] <= LTRIM(RTRIM(@Name))) " 
                    + "AND   (@Quantity IS NULL OR @Quantity = '' OR [Products].[Quantity] <= LTRIM(RTRIM(@Quantity))) " 
                    + "AND   (@BoxSize IS NULL OR @BoxSize = '' OR [Products].[BoxSize] <= LTRIM(RTRIM(@BoxSize))) " 
                    + "AND   (@Price IS NULL OR @Price = '' OR [Products].[Price] <= LTRIM(RTRIM(@Price))) " 
                    + "";
            }
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.CommandType = CommandType.Text;
            if (sField == "Product Id") {
                selectCommand.Parameters.AddWithValue("@ProductId", sValue);
            } else {
                selectCommand.Parameters.AddWithValue("@ProductId", DBNull.Value); }
            if (sField == "Name") {
                selectCommand.Parameters.AddWithValue("@Name", sValue);
            } else {
                selectCommand.Parameters.AddWithValue("@Name", DBNull.Value); }
            if (sField == "Quantity") {
                selectCommand.Parameters.AddWithValue("@Quantity", sValue);
            } else {
                selectCommand.Parameters.AddWithValue("@Quantity", DBNull.Value); }
            if (sField == "Box Size") {
                selectCommand.Parameters.AddWithValue("@BoxSize", sValue);
            } else {
                selectCommand.Parameters.AddWithValue("@BoxSize", DBNull.Value); }
            if (sField == "Price") {
                selectCommand.Parameters.AddWithValue("@Price", sValue);
            } else {
                selectCommand.Parameters.AddWithValue("@Price", DBNull.Value); }
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                if (reader.HasRows) {
                    dt.Load(reader); }
                reader.Close();
            }
            catch (SqlException)
            {
                return dt;
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static Products Select_Record(Products productsPara)
        {
        Products products = new Products();
            SqlConnection connection = TutorialsteamData.GetConnection();
            string selectStatement
            = "SELECT " 
                + "     [ProductId] "
                + "    ,[Name] "
                + "    ,[Quantity] "
                + "    ,[BoxSize] "
                + "    ,[Price] "
                + "FROM "
                + "     [Products] "
                + "WHERE "
                + "     [ProductId] = @ProductId "
                + "";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.CommandType = CommandType.Text;
            selectCommand.Parameters.AddWithValue("@ProductId", productsPara.ProductId);
            try
            {
                connection.Open();
                SqlDataReader reader
                    = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    products.ProductId = System.Convert.ToInt32(reader["ProductId"]);
                    products.Name = System.Convert.ToString(reader["Name"]);
                    products.Quantity = System.Convert.ToInt32(reader["Quantity"]);
                    products.BoxSize = System.Convert.ToInt32(reader["BoxSize"]);
                    products.Price = System.Convert.ToDecimal(reader["Price"]);
                }
                else
                {
                    products = null;
                }
                reader.Close();
            }
            catch (SqlException)
            {
                return products;
            }
            finally
            {
                connection.Close();
            }
            return products;
        }

        public static bool Add(Products Products)
        {
            SqlConnection connection = TutorialsteamData.GetConnection();
            string insertStatement
                = "INSERT " 
                + "     [Products] "
                + "     ( "
                + "     [Name] "
                + "    ,[Quantity] "
                + "    ,[BoxSize] "
                + "    ,[Price] "
                + "     ) "
                + "VALUES " 
                + "     ( "
                + "     @Name "
                + "    ,@Quantity "
                + "    ,@BoxSize "
                + "    ,@Price "
                + "     ) "
                + "";
            SqlCommand insertCommand = new SqlCommand(insertStatement, connection);
            insertCommand.CommandType = CommandType.Text;
                insertCommand.Parameters.AddWithValue("@Name", Products.Name);
                insertCommand.Parameters.AddWithValue("@Quantity", Products.Quantity);
                insertCommand.Parameters.AddWithValue("@BoxSize", Products.BoxSize);
                insertCommand.Parameters.AddWithValue("@Price", Products.Price);
            try
            {
                connection.Open();
                int count = insertCommand.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool Update(Products oldProducts, 
               Products newProducts)
        {
            SqlConnection connection = TutorialsteamData.GetConnection();
            string updateStatement
                = "UPDATE "  
                + "     [Products] "
                + "SET "
                + "     [Name] = @NewName "
                + "    ,[Quantity] = @NewQuantity "
                + "    ,[BoxSize] = @NewBoxSize "
                + "    ,[Price] = @NewPrice "
                + "WHERE "
                + "     [ProductId] = @OldProductId " 
                + " AND [Name] = @OldName " 
                + " AND [Quantity] = @OldQuantity " 
                + " AND [BoxSize] = @OldBoxSize " 
                + " AND [Price] = @OldPrice " 
                + "";
            SqlCommand updateCommand = new SqlCommand(updateStatement, connection);
            updateCommand.CommandType = CommandType.Text;
            updateCommand.Parameters.AddWithValue("@NewName", newProducts.Name);
            updateCommand.Parameters.AddWithValue("@NewQuantity", newProducts.Quantity);
            updateCommand.Parameters.AddWithValue("@NewBoxSize", newProducts.BoxSize);
            updateCommand.Parameters.AddWithValue("@NewPrice", newProducts.Price);
            updateCommand.Parameters.AddWithValue("@OldProductId", oldProducts.ProductId);
            updateCommand.Parameters.AddWithValue("@OldName", oldProducts.Name);
            updateCommand.Parameters.AddWithValue("@OldQuantity", oldProducts.Quantity);
            updateCommand.Parameters.AddWithValue("@OldBoxSize", oldProducts.BoxSize);
            updateCommand.Parameters.AddWithValue("@OldPrice", oldProducts.Price);
            try
            {
                connection.Open();
                int count = updateCommand.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool Delete(Products Products)
        {
            SqlConnection connection = TutorialsteamData.GetConnection();
            string deleteStatement
                = "DELETE FROM "  
                + "     [Products] "
                + "WHERE " 
                + "     [ProductId] = @OldProductId " 
                + " AND [Name] = @OldName " 
                + " AND [Quantity] = @OldQuantity " 
                + " AND [BoxSize] = @OldBoxSize " 
                + " AND [Price] = @OldPrice " 
                + "";
            SqlCommand deleteCommand = new SqlCommand(deleteStatement, connection);
            deleteCommand.CommandType = CommandType.Text;
            deleteCommand.Parameters.AddWithValue("@OldProductId", Products.ProductId);
            deleteCommand.Parameters.AddWithValue("@OldName", Products.Name);
            deleteCommand.Parameters.AddWithValue("@OldQuantity", Products.Quantity);
            deleteCommand.Parameters.AddWithValue("@OldBoxSize", Products.BoxSize);
            deleteCommand.Parameters.AddWithValue("@OldPrice", Products.Price);
            try
            {
                connection.Open();
                int count = deleteCommand.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
 
