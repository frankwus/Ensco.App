using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using AutoMapper.Configuration.Conventions;
using Ensco.Models;
using System.Linq.DataExtensions;
using Ensco.Utilities;
using Ensco.Irma.Data;
using mUtilities.Data;
namespace Ensco.Irma.Services
{
    public class IrmaServiceDataModel<M, T> : IIrmaServiceDataModel where M:class where T:class
    {
        EnscoIrmaRepository<T> entities = new EnscoIrmaRepository<T>();
        MapperConfiguration mapperConfiguration = null;

        public IrmaServiceDataModel()
        {

            try
            {
                // Make sure to add field name to property name mapping here from columnattribs
                mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<T, M>().ReverseMap();

                    cfg.CreateMissingTypeMaps = true;

                    //CreateNested(cfg, typeof(T), typeof(M));
                });
                mapperConfiguration.CreateMapper();
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }
        }
        private void CreateNested(IMapperConfigurationExpression cfg, Type source, Type dest)
        {
            PropertyInfo[] srcProps = source.GetProperties();
            PropertyInfo[] destProps = dest.GetProperties();
            foreach (PropertyInfo prop in srcProps)
            {
                PropertyInfo destProp = destProps.FirstOrDefault(x => x.Name == prop.Name);

                //
                // Check here to see if this is UserDefined class and call CreatedNested recursively
                // Otherwise, use the following to Create the map for AutoMapper.
                //
                if (destProp != null && destProp.PropertyType.IsGenericType)
                {
                        cfg.CreateMap(prop.PropertyType, destProp.PropertyType).ReverseMap();
                    //else
                     //   cfg.CreateMap(prop.PropertyType, destProp.PropertyType).ReverseMap();
                }
            }
        }
        public Dictionary<string, string> ServerInfo()
        {
            return entities.ServerInfo();
        }

        public IQueryable GetQueryable(string sortColumn)
        {
            IQueryable<M> result = null;

            try
            {
                result = entities.GetAll().ProjectTo<M>(mapperConfiguration);
                result = result.OrderBy(sortColumn);
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message, sortColumn));
            }

            return result;
        }

        public IQueryable GetQueryable(string filter, string sort)
        {
            IQueryable<M> result = null;

            try
            {
                result = entities.GetAll().ProjectTo<M>(mapperConfiguration);
                result = result.Where(filter);
                result = result.OrderBy(sort);
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message, sort));
            }

            return result;
        }

        public List<dynamic> GetAllItems()
        {
            List<dynamic> items = new List<dynamic>();

            try
            {
                List<M> dbitems = entities.GetAll().ProjectTo<M>(mapperConfiguration).ToList();
                items = dbitems.Cast<dynamic>().ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }

            return items;
        }

        public List<dynamic> GetItems(string filter, string sort)
        {
            List<dynamic> list = new List<dynamic>();

            IQueryable queryable = GetQueryable(filter, sort);
            if (queryable != null)
            {
                foreach (dynamic item in queryable)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public dynamic GetItem(string filter, string sort)
        {
            dynamic item = null;

            IQueryable queryable = GetQueryable(filter, sort);
            if(queryable != null)
            {
                foreach(dynamic obj in queryable)
                {
                    item = obj;
                    break;
                }
            }

            return item;
        }

        public List<dynamic> GetItems(Expression filter)
        {
            List<dynamic> items = new List<dynamic>();

            try
            {
                List<M> dbitems = entities.GetAll().ProjectTo<M>(mapperConfiguration).ToList();
                items = dbitems.FindAll(filter as Predicate<M>).Cast<dynamic>().ToList();

            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }

            return items;
        }

        public dynamic Refresh(dynamic item)
        {
            try
            {
                IMapper iMapper = mapperConfiguration.CreateMapper();
                M model = (M)item;
                T entity = iMapper.Map<M, T>(model);

                entity = entities.Refresh(entity);
                item = iMapper.Map<T, M>(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message, item));
                return null;
            }

            return item;
        }

        public dynamic Add(dynamic item, bool commit)
        {
            try
            {
                IMapper iMapper = mapperConfiguration.CreateMapper();
                M model = (M)item;
                T entity = iMapper.Map<M, T>(model);
                entities.Add(entity);

                if (commit)
                    entities.Save();

                entities.Reload(entity);
                item = iMapper.Map<T, M>(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message, item));
            }

            return item;
        }

        public bool Update(dynamic item, bool commit)
        {
            try
            {
                IMapper iMapper = mapperConfiguration.CreateMapper();
                M model = (M)item;
                T entity = iMapper.Map<M, T>(model);
                entities.Edit(entity);

                if (commit)
                    entities.Save();
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message, item));
                return false;
            }

            return true;
        }

        public bool Delete(dynamic item, bool commit)
        {
            try
            {
                IMapper iMapper = mapperConfiguration.CreateMapper();
                M model = (M)item;
                T entity = iMapper.Map<M, T>(model);
                entities.Delete(entity);

                if (commit)
                    entities.Save();
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message, item));
            }

            return true;
        }

        // If you are using a transaction, do all the operations and call Save method to commit the transaction
        public bool Save()
        {
            try
            {
                entities.Save();
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
                return false;
            }

            return true;
        }
    }
}
