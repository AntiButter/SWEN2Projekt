using System.Linq;
using System.Collections.Generic;
using TourPlanner.Models;
using TourPlanner.DataAccessLayer;
namespace TourPlanner.BusinessLayer
{
    internal class MediaItemFactoryImpl : IMediaItemFactory
    {
        private MediaItemDAO mediaItemDAO = new MediaItemDAO();

        public IEnumerable<MediaItem> Search(string itemName, bool caseSensitive = false)
        {
            IEnumerable<MediaItem> items = GetItems();
            if(caseSensitive)
            {
                return items.Where(x => x.Name.Contains(itemName));

            }
            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }

        public IEnumerable<MediaItem> GetItems()
        {
            return mediaItemDAO.GetItems(); 
        }
    }
}
