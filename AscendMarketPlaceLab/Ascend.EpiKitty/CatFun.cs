﻿using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Cache;
using EPiServer.Framework.Web.Resources;
using EPiServer.Web.Routing;
using System;
using System.Linq;
using System.Web;

namespace Ascend.EpiKitty
{
    [ClientResourceRegistrator]
    public class CatFun : IClientResourceRegistrator
    {
        private IContentRouteHelper _routeHelper;
        private IContentRepository _repo;
        private ISynchronizedObjectInstanceCache _cache;

        public ContentReference HidingPlace
        {
            get
            {
                return new ContentReference(5, true);
                var cref = _cache.Get<ContentReference>("Kitty", ReadStrategy.Immediate);
                if (cref == null)
                {
                    var lst = _repo.GetDescendents(ContentReference.StartPage).Take(200).ToList();
                    Random R = new Random();
                    cref = lst[R.Next(lst.Count)];
                    _cache.Insert("Kitty", cref, new CacheEvictionPolicy(new TimeSpan(2, 0, 0), CacheTimeoutType.Absolute));
                }
                return cref;
            }
        }

        public CatFun(IContentRouteHelper crh, IContentRepository repo, ISynchronizedObjectInstanceCache cache)
        {
            _routeHelper = crh;
            _repo = repo;
            _cache = cache;
        }

        public void RegisterResources(IRequiredClientResourceList requiredResources)
        {
            if (HttpContext.Current.User.IsInRole("WebAdmins") || HttpContext.Current.User.IsInRole("WebEditors"))
            {
                var content = _routeHelper.Content;
                if (content!=null && content.ContentLink.CompareToIgnoreWorkID(HidingPlace))
                {
                    requiredResources.RequireScriptInline("const kittypath='"+EPiServer.Shell.Paths.ToResource("Ascend.EpiKitty","kitten.png")+"';");
                    requiredResources.RequireScript(EPiServer.Shell.Paths.ToResource("Ascend.EpiKitty","catfun.js"));
                }
            }
        }
    }
}