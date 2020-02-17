using HistoryYoutubeVideosApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HistoryYoutubeVideosApi.Controllers
{
    public class HistoryController : ApiController
    {
        YouTubeEntities dbContext = new YouTubeEntities();

        //GET api/History
        [HttpGet]
        public List<PlayedVideoDto> PlayedVideos(int amount = 10)
        {
            var lastVideosWatched = dbContext.History
                .Where(c => c.IsEnabled != false)
                .OrderByDescending(p => p.Id)
                .Take(amount)
                .Select(h => new PlayedVideoDto
            {
                Id = h.Id,
                VideoId = h.VideoId,
                IsEnabled = h.IsEnabled,
                Date = h.Date
            });

            return lastVideosWatched.ToList();
            
        }

        //POST api/History
        [HttpPost]
       public PlayedVideoDto PostVideo( string VideoId)
        {

            History video  = new History();
            video.VideoId = VideoId;
            video.IsEnabled = true;
            video.Date = DateTime.Now;
            dbContext.History.Add(video);
            dbContext.SaveChanges();
            var videoGuardado = dbContext.History.Where(c => c.VideoId == VideoId).Select(x => new PlayedVideoDto
            {
                Id = x.Id,
                VideoId = x.VideoId,
                IsEnabled = x.IsEnabled,
                Date = x.Date
            }).ToList();
            var v = videoGuardado[0];
            return v;

        }

        //DELETE api/History
        [HttpDelete]
        public void  DeleteHistoryVideo(int Id)
        {
            var videoRemoved = dbContext.History.FirstOrDefault(v => v.Id == Id);
            if (videoRemoved != null)
                videoRemoved.IsEnabled = false;
            dbContext.SaveChanges();
            

        }







    }
}
