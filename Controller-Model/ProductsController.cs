using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using ProductSQLTest2.Models;
using ProductSQLTest2.Data;

namespace ProductSQLTest2.Controllers
{
    public class ProductsController : Controller
    {

        DataTable dtProducts = new DataTable();

        // GET: /Products/
        public ActionResult Index(string sortOrder,  
                                  String SearchField,
                                  String SearchCondition,
                                  String SearchText,
                                  String Export,
                                  int? PageSize,
                                  int? page, 
                                  string command)
        {

            if (command == "Show All") {
                SearchField = null;
                SearchCondition = null;
                SearchText = null;
                Session["SearchField"] = null;
                Session["SearchCondition"] = null;
                Session["SearchText"] = null; } 
            else if (command == "Add New Record") { return RedirectToAction("Create"); } 
            else if (command == "Export") { Session["Export"] = Export; } 
            else if (command == "Search" | command == "Page Size") {
                if (!string.IsNullOrEmpty(SearchText)) {
                    Session["SearchField"] = SearchField;
                    Session["SearchCondition"] = SearchCondition;
                    Session["SearchText"] = SearchText; }
                } 
            if (command == "Page Size") { Session["PageSize"] = PageSize; }

            ViewData["SearchFields"] = GetFields((Session["SearchField"] == null ? "Product Id" : Convert.ToString(Session["SearchField"])));
            ViewData["SearchConditions"] = Library.GetConditions((Session["SearchCondition"] == null ? "Contains" : Convert.ToString(Session["SearchCondition"])));
            ViewData["SearchText"] = Session["SearchText"];
            ViewData["Exports"] = Library.GetExports((Session["Export"] == null ? "Pdf" : Convert.ToString(Session["Export"])));
            ViewData["PageSizes"] = Library.GetPageSizes();

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ProductIdSortParm"] = sortOrder == "ProductId_asc" ? "ProductId_desc" : "ProductId_asc";
            ViewData["NameSortParm"] = sortOrder == "Name_asc" ? "Name_desc" : "Name_asc";
            ViewData["QuantitySortParm"] = sortOrder == "Quantity_asc" ? "Quantity_desc" : "Quantity_asc";
            ViewData["BoxSizeSortParm"] = sortOrder == "BoxSize_asc" ? "BoxSize_desc" : "BoxSize_asc";
            ViewData["PriceSortParm"] = sortOrder == "Price_asc" ? "Price_desc" : "Price_asc";

            dtProducts = ProductsData.SelectAll();

            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["SearchField"])) & !string.IsNullOrEmpty(Convert.ToString(Session["SearchCondition"])) & !string.IsNullOrEmpty(Convert.ToString(Session["SearchText"])))
                {
                    dtProducts = ProductsData.Search(Convert.ToString(Session["SearchField"]), Convert.ToString(Session["SearchCondition"]), Convert.ToString(Session["SearchText"]));
                }
            }
            catch { }

            var Query = from rowProducts in dtProducts.AsEnumerable()
                        select new Products() {
                            ProductId = rowProducts.Field<Int32>("ProductId")
                           ,Name = rowProducts.Field<String>("Name")
                           ,Quantity = rowProducts.Field<Int32>("Quantity")
                           ,BoxSize = rowProducts.Field<Int32>("BoxSize")
                           ,Price = rowProducts.Field<Decimal>("Price")
                        };

            switch (sortOrder)
            {
                case "ProductId_desc":
                    Query = Query.OrderByDescending(s => s.ProductId);
                    break;
                case "ProductId_asc":
                    Query = Query.OrderBy(s => s.ProductId);
                    break;
                case "Name_desc":
                    Query = Query.OrderByDescending(s => s.Name);
                    break;
                case "Name_asc":
                    Query = Query.OrderBy(s => s.Name);
                    break;
                case "Quantity_desc":
                    Query = Query.OrderByDescending(s => s.Quantity);
                    break;
                case "Quantity_asc":
                    Query = Query.OrderBy(s => s.Quantity);
                    break;
                case "BoxSize_desc":
                    Query = Query.OrderByDescending(s => s.BoxSize);
                    break;
                case "BoxSize_asc":
                    Query = Query.OrderBy(s => s.BoxSize);
                    break;
                case "Price_desc":
                    Query = Query.OrderByDescending(s => s.Price);
                    break;
                case "Price_asc":
                    Query = Query.OrderBy(s => s.Price);
                    break;
                default:  // Name ascending 
                    Query = Query.OrderBy(s => s.ProductId);
                    break;
            }

            if (command == "Export") {
                GridView gv = new GridView();
                DataTable dt = new DataTable();
                dt.Columns.Add("Product Id", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Quantity", typeof(string));
                dt.Columns.Add("Box Size", typeof(string));
                dt.Columns.Add("Price", typeof(string));
                foreach (var item in Query)
                {
                    dt.Rows.Add(
                        item.ProductId
                       ,item.Name
                       ,item.Quantity
                       ,item.BoxSize
                       ,item.Price
                    );
                }
                gv.DataSource = dt;
                gv.DataBind();
                ExportData(Export, gv, dt);
            }

            int pageNumber = (page ?? 1);
            int? pageSZ = (Convert.ToInt32(Session["PageSize"]) == 0 ? 5 : Convert.ToInt32(Session["PageSize"]));
            return View(Query.ToPagedList(pageNumber, (pageSZ ?? 5)));
        }

        // GET: /Products/Details/<id>
        public ActionResult Details(
                                      Int32? ProductId
                                   )
        {
            if (
                    ProductId == null
               )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Products Products = new Products();
            Products.ProductId = System.Convert.ToInt32(ProductId);
            Products = ProductsData.Select_Record(Products);

            if (Products == null)
            {
                return HttpNotFound();
            }
            return View(Products);
        }

        // GET: /Products/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: /Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include=
				           "Name"
				   + "," + "Quantity"
				   + "," + "BoxSize"
				   + "," + "Price"
				  )] Products Products)
        {
            if (ModelState.IsValid)
            {
                bool bSucess = false;
                bSucess = ProductsData.Add(Products);
                if (bSucess == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Can Not Insert");
                }
            }

            return View(Products);
        }

        // GET: /Products/Edit/<id>
        public ActionResult Edit(
                                   Int32? ProductId
                                )
        {
            if (
                    ProductId == null
               )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Products Products = new Products();
            Products.ProductId = System.Convert.ToInt32(ProductId);
            Products = ProductsData.Select_Record(Products);

            if (Products == null)
            {
                return HttpNotFound();
            }

            return View(Products);
        }

        // POST: /Products/Edit/<id>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products Products)
        {

            Products oProducts = new Products();
            oProducts.ProductId = System.Convert.ToInt32(Products.ProductId);
            oProducts = ProductsData.Select_Record(Products);

            if (ModelState.IsValid)
            {
                bool bSucess = false;
                bSucess = ProductsData.Update(oProducts, Products);
                if (bSucess == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Can Not Update");
                }
            }

            return View(Products);
        }

        // GET: /Products/Delete/<id>
        public ActionResult Delete(
                                     Int32? ProductId
                                  )
        {
            if (
                    ProductId == null
               )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Products Products = new Products();
            Products.ProductId = System.Convert.ToInt32(ProductId);
            Products = ProductsData.Select_Record(Products);

            if (Products == null)
            {
                return HttpNotFound();
            }
            return View(Products);
        }

        // POST: /Products/Delete/<id>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(
                                            Int32? ProductId
                                            )
        {

            Products Products = new Products();
            Products.ProductId = System.Convert.ToInt32(ProductId);
            Products = ProductsData.Select_Record(Products);

            bool bSucess = false;
            bSucess = ProductsData.Delete(Products);
            if (bSucess == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Can Not Delete");
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private static List<SelectListItem> GetFields(String select)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            SelectListItem Item1 = new SelectListItem { Text = "Product Id", Value = "Product Id" };
            SelectListItem Item2 = new SelectListItem { Text = "Name", Value = "Name" };
            SelectListItem Item3 = new SelectListItem { Text = "Quantity", Value = "Quantity" };
            SelectListItem Item4 = new SelectListItem { Text = "Box Size", Value = "Box Size" };
            SelectListItem Item5 = new SelectListItem { Text = "Price", Value = "Price" };

                 if (select == "Product Id") { Item1.Selected = true; }
            else if (select == "Name") { Item2.Selected = true; }
            else if (select == "Quantity") { Item3.Selected = true; }
            else if (select == "Box Size") { Item4.Selected = true; }
            else if (select == "Price") { Item5.Selected = true; }

            list.Add(Item1);
            list.Add(Item2);
            list.Add(Item3);
            list.Add(Item4);
            list.Add(Item5);

            return list.ToList();
        }

        private void ExportData(String Export, GridView gv, DataTable dt)
        {
            if (Export == "Pdf")
            {
                PDFform pdfForm = new PDFform(dt, "Dbo. Products", "Many");
                Document document = pdfForm.CreateDocument();
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = document;
                renderer.RenderDocument();

                MemoryStream stream = new MemoryStream();
                renderer.PdfDocument.Save(stream, false);

                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + "Report.pdf");
                Response.ContentType = "application/Pdf.pdf";
                Response.BinaryWrite(stream.ToArray());
                Response.Flush();
                Response.End();
            }
            else
            {
                Response.ClearContent();
                Response.Buffer = true;
                if (Export == "Excel")
                {
                    Response.AddHeader("content-disposition", "attachment;filename=" + "Report.xls");
                    Response.ContentType = "application/Excel.xls";
                }
                else if (Export == "Word")
                {
                    Response.AddHeader("content-disposition", "attachment;filename=" + "Report.doc");
                    Response.ContentType = "application/Word.doc";
                }
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

    }
}
 
