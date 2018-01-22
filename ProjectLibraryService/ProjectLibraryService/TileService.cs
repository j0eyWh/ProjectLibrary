using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using NotificationsExtensions;
using NotificationsExtensions.Tiles;
using NotificationsExtensions.Toasts;

namespace ProjectLibraryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TileService" in both code and config file together.
    public class TileService : ITileService
    {
        private LibraryService service = new LibraryService();

        public XmlElement UpdateTile(string userId)
        {
            int id;
            if (!Int32.TryParse(userId, out id))
                throw new ArgumentException("Wrong argument! Int was expected!");

            var user = service.DbContext.Users.FirstOrDefault(x => x.UserId == id);

            if (user==null)
                throw new Exception("No such user found!");

            int expiredReservationsCount =
                service.DbContext.ReservedBooks.Where(x=>x.UserId==id).ToList().Count(x=>x.TimeOut - DateTime.Now <= TimeSpan.FromDays(1));

            int expiredOnHandsCount = service.DbContext.OnHandsBooks.Where(x => x.UserId == id).ToList()
                .Count(x=>x.ReturnDate -DateTime.Now <= TimeSpan.FromDays(1));

            int totalOnHandsCount = service.DbContext.OnHandsBooks.Count(x => x.UserId == id);
            int totalReservedCount = service.DbContext.ReservedBooks.Count(x => x.UserId == id);



            var xmlDocument = new XmlDocument();



            TileContent tileContent = new TileContent()
            {
                Visual = new TileVisual()
                {
                    AddImageQuery = true,
                    TileWide = new TileBinding()
                    {
                        Branding = TileBranding.Name,

                        Content = new TileBindingContentAdaptive()
                        {
                            PeekImage = new TilePeekImage()
                            {
                                Source = "http://localhost:8733/Design_Time_Addresses/ProjectLibraryService/TileService/GetImage",
                                HintCrop = TilePeekImageCrop.Circle
                            },
                           

                            Children =
                            {
                                new TileText()
                                    {
                                        Text = $"Hey, {user.Name}!",
                                        Style = TileTextStyle.Title
                                    }
                                
                            }
                        }
                    },
                    TileMedium = new TileBinding()
                    {
                        Branding = TileBranding.Logo,
                        Content = new TileBindingContentAdaptive()
                        {
                            PeekImage = new TilePeekImage()
                            {
                                Source = "http://localhost:8733/Design_Time_Addresses/ProjectLibraryService/TileService/GetImage",
                                HintCrop = TilePeekImageCrop.Circle
                            },


                            Children =
                            {
                                new TileText()
                                    {
                                        Text = $"Hey, {user.Name}!",
                                        Style = TileTextStyle.Title
                                    }

                            }
                        }
                    }

                }
            };

            if (expiredReservationsCount>0)
            {
                ((TileBindingContentAdaptive)tileContent.Visual.TileWide.Content).Children.Add(new TileText()
                {
                    Text = $"You have {expiredReservationsCount} reservations that are about to expire",
                    Wrap = true
                });

                ((TileBindingContentAdaptive)tileContent.Visual.TileWide.Content).Children.Add(new TileText()
                {
                    Text = $"You have {expiredReservationsCount} reservations that are about to expire",
                    Wrap = true
                });
            }
            if (expiredOnHandsCount> 0)
            {
                ((TileBindingContentAdaptive)tileContent.Visual.TileWide.Content).Children.Add(new TileText()
                {
                    Text = $"You have {expiredOnHandsCount} books that you must return",
                    Wrap = true
                });

                ((TileBindingContentAdaptive)tileContent.Visual.TileMedium.Content).Children.Add(new TileText()
                {
                    Text = $"You have {expiredOnHandsCount} books that you must return",
                    Wrap = true
                });
            }

            ((TileBindingContentAdaptive)tileContent.Visual.TileWide.Content).Children.Add(new TileText()
            {
                Text = $"You're reading {totalOnHandsCount} books",
                Wrap = true
            });

            ((TileBindingContentAdaptive)tileContent.Visual.TileWide.Content).Children.Add(new TileText()
            {
                Text = $"You have {totalReservedCount} reservations",
                Wrap = true
            });


            ((TileBindingContentAdaptive)tileContent.Visual.TileMedium.Content).Children.Add(new TileText()
            {
                Text = $"You're reading {totalOnHandsCount} books",
                Wrap = true
            });

            ((TileBindingContentAdaptive)tileContent.Visual.TileMedium.Content).Children.Add(new TileText()
            {
                Text = $"You have {totalReservedCount} reservations",
                Wrap = true
            });

            xmlDocument.LoadXml(tileContent.GetContent());

            return xmlDocument.DocumentElement;
        }

        public XmlElement GetNotification(string userId)
        {
            int id;
            if (!Int32.TryParse(userId, out id))
                throw new ArgumentException("Wrong argument! Int was expected!");

            var user = service.DbContext.Users.FirstOrDefault(x => x.UserId == id);

            if (user == null)
                throw new Exception("No such user found!");

            int expiredReservationsCount =
                service.DbContext.ReservedBooks.Where(x => x.UserId == id).ToList().Count(x => x.TimeOut - DateTime.Now <= TimeSpan.FromDays(1));

            int expiredOnHandsCount = service.DbContext.OnHandsBooks.Where(x => x.UserId == id).ToList()
                .Count(x => x.ReturnDate - DateTime.Now <= TimeSpan.FromDays(1));

            var notification = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText() {Text = $"Hey, {user.Name}!"}
                        }
                    }
                    
                }
            };


            if (expiredReservationsCount>0)
            {
                string text = expiredReservationsCount==1 ? $"You have one reservation that will expire soon!" 
                    : $"You have {expiredReservationsCount} that will expire soon!";

                notification.Visual.BindingGeneric.Children.Add(new AdaptiveText() {Text=text});
            }

            if (expiredOnHandsCount >0)
            {
                string text;

                if (expiredOnHandsCount == 1)
                {
                    var bookTitle = service.DbContext.OnHandsBooks.Where(x => x.UserId == user.UserId)
                        .Include(x => x.BookCode.Book)
                        .FirstOrDefault()
                        .BookCode.Book.Title;
                    text = $"You should return \"{bookTitle}\" as soon as possible!";
                }
                else
                {
                    text = $"You have {expiredOnHandsCount} books that you should return!";
                }

                notification.Visual.BindingGeneric.Children.Add(new AdaptiveText() { Text = text });

            }

            var xmlDocument = new XmlDocument();

            xmlDocument.LoadXml(notification.GetContent());

            return xmlDocument.DocumentElement;

        }

        public Stream GetImage()
        {
            var userImg = (new LibraryService()).DbContext.Users.FirstOrDefault(x=>x.UserId == 1).UserPic;
            Stream fs = new MemoryStream(userImg);
            WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";
            return fs;
        
        }

    }
}
