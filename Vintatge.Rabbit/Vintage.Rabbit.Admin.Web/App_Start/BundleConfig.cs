﻿using System.Web;
using System.Web.Optimization;

namespace Vintage.Rabbit.Admin.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Content/scripts/jquery/jquery-{version}.js").Include("~/Content/scripts/jquery/jquery-ui-1.10.4.custom.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Content/scripts/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Content/scripts/modernizr/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/scripts")
                .Include("~/Content/scripts/bootstrap/*.js")
                .Include("~/Content/scripts/datepicker/*.js")
                .Include("~/Content/scripts/photo-uploader/*.js")
                .Include("~/Content/scripts/lightbox/*.js")
                .Include("~/Content/scripts/*.js"));


            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
        }
    }
}
