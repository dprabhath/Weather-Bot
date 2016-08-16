using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Bot = Microsoft.Bot.Connector;

namespace Bot_Application2
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public Message Post([FromBody]Message message)
        {


            Message replyMessage = message.CreateReplyMessage("");

            if (message.Type == "Message")
            {



                if (message.Text == "hi" || message.Text == "HI" || message.Text == "Hi")
                {
                    return message.CreateReplyMessage("Hi, I can let you know the weather report of your city/town. If you are ready, send me your location. Make no sense ? Then type 'help'");
                }

                else if (Regex.IsMatch(message.Text, "hello", RegexOptions.IgnoreCase))
                {
                    return message.CreateReplyMessage("Hello, I can let you know the weather report of your city/town. If you are ready, send me your location. Make no sense ? Then type 'help'");
                }

                else if (Regex.IsMatch(message.Text, "hey", RegexOptions.IgnoreCase))
                {
                    return message.CreateReplyMessage("Hey, I can let you know the weather report of your city/town. If you are ready, send me your location. Make no sense ? Then type 'help'");
                }
                else if (Regex.IsMatch(message.Text, "how", RegexOptions.IgnoreCase))
                {
                    return message.CreateReplyMessage("Hey, I can let you know the weather report of your city/town. If you are ready, send me your location. Make no sense ? Then type 'help'");
                }


             




                else {

                    string results = "";


                    using (System.Net.WebClient wc = new WebClient())
                    {
                        results = wc.DownloadString("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22" + message.Text + "%2C%20ak%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
                    }


                    dynamic jo = JObject.Parse(results);
                    var items = jo.query.results.channel;

                    String count = jo.query.count;
                    int count1 = Convert.ToInt32(count);

                    //var msg = jo.error;

                    //String msg1 = Convert.ToString(msg.lang);

                    bool con = message.Text.All(Char.IsLetterOrDigit);

                    if (con == false)
                    {
                        // return message.CreateReplyMessage("Sorry. I could not find a weather report for " + message.Text + ". Can you try again with an another nearby location or with your zip code." + "\n");

                        replyMessage.Attachments = new List<Attachment>();

                        replyMessage.Attachments.Add(new Attachment()
                        {
                            Title = "Not Found!",
                            // TitleLink = "https://en.wikipedia.org/wiki/Bender_(Futurama)",
                            ThumbnailUrl = "http://windowsgeek.lk/dpm/not.jpg",
                            Text = "I could not find a weather report for " + message.Text + ". Try again with a nearby city.",
                            FallbackText = "I could not find a weather report for " + message.Text + ". Try again with a nearby city."
                        });

                        return replyMessage;



                    }

            




                    else if (Regex.IsMatch(message.Text, "help", RegexOptions.IgnoreCase))
                    {
                        //return message.CreateReplyMessage("It is so easy. To get the weather report, just enter your location or the zip code and press enter" + "\n");



                        replyMessage.Attachments = new List<Attachment>();

                        replyMessage.Attachments.Add(new Attachment()
                        {
                            Title = "Help",
                            // TitleLink = "https://en.wikipedia.org/wiki/Bender_(Futurama)",
                            ThumbnailUrl = "http://windowsgeek.lk/dpm/help.jpg",
                            Text = "Enter your city or the zip code and press enter",
                            FallbackText = "Enter your city or the zip code and press enter"
                        });



                        var actions = new List<Microsoft.Bot.Connector.Action>();

                        actions.Add(new Microsoft.Bot.Connector.Action
                        {
                            Title = "Kaduwela",
                            Message = "kaduwela"
                        });



                        actions.Add(new Microsoft.Bot.Connector.Action
                        {
                            Title = "Jaffna",
                            Message = "jaffna"
                        });
                        actions.Add(new Microsoft.Bot.Connector.Action
                        {
                            Title = "London",
                            Message = "london"
                        });


                        replyMessage.Attachments.Add(new Attachment
                        {
                            Title = "Try these examples,",
                            Actions = actions
                        });

                        return replyMessage;
















                    }





                    else
                    {




                        if (count != "0")
                        {


                            String title = items.item.title;
                            String date = items.lastBuildDate;
                            String winddirection = items.wind.direction;
                            String speed = items.wind.speed;
                            String humidity = items.atmosphere.humidity;
                            String sunrise = items.astronomy.sunrise;
                            String sunset = items.astronomy.sunset;
                            String imgurl = items.image.url;
                            String temp = items.item.condition.temp;
                            String condition = items.item.condition.text;
                            String city = items.location.city;



                            //tomorrow forecast



                            String hightemp = items.item.forecast[1].high;
                            String lowtemp = items.item.forecast[1].low;
                            String condi = items.item.forecast[1].text;




                            if (city == "Alaska")
                            {

                                replyMessage.Attachments = new List<Attachment>();

                                replyMessage.Attachments.Add(new Attachment()
                                {
                                    Title = "Not Found!",
                                    // TitleLink = "https://en.wikipedia.org/wiki/Bender_(Futurama)",
                                    ThumbnailUrl = "http://windowsgeek.lk/dpm/not.jpg",
                                    Text = "I could not find a weather report for " + message.Text + ". Try again with a nearby city.",
                                    FallbackText = "I could not find a weather report for " + message.Text + ". Can you try again with another nearby city."
                                });

                                return replyMessage;


                            }




                            else
                            {

                                replyMessage.Attachments = new List<Attachment>();




                                replyMessage.Attachments.Add(new Attachment()
                                {
                                    Title = "Weather " + title,

                                    //TitleLink = "https://en.wikipedia.org/wiki/Bender_(Futurama)",
                                    ThumbnailUrl = "http://windowsgeek.lk/dpm/ww.jpg",

                                    Text = " Mainly " + condition + " condition will be there." + " wind speed at " + speed + " mph.",
                                    FallbackText = " Mainly " + condition + " condition will be there." + " wind speed at " + speed + " mph. Temperature is " + temp + " °F" + "."
                                });



                                replyMessage.Attachments.Add(new Attachment()
                                {
                                    Title = "Weather " + title,
                                    ThumbnailUrl = "http://windowsgeek.lk/dpm/ww.jpg",
                                    Text = " Temperature is " + temp + " °F" + ".",
                                    FallbackText = " Temperature is " + temp + " °F" + "."
                                });

                                replyMessage.Attachments.Add(new Attachment()
                                {
                                    Title = "Weather " + title,
                                    ThumbnailUrl = "http://windowsgeek.lk/dpm/ww.jpg",
                                    Text = " Humidity is " + humidity + "%" + ". Sunrise in " + city + " at " + sunrise + " and Sunset at " + sunset + ".",
                                    FallbackText = " Humidity is " + humidity + "%" + ". Sunrise in " + city + " at " + sunrise + " and Sunset at " + sunset + "."
                                });


                                replyMessage.Attachments.Add(new Attachment()
                                {
                                    Title = "Tomorrow weather forecast of " + city,
                                    //TitleLink = "https://en.wikipedia.org/wiki/Bender_(Futurama)",
                                    ThumbnailUrl = "http://windowsgeek.lk/dpm/forecast.jpg",

                                    Text = " Generaly there will be a " + condi + " condition.",
                                    FallbackText = " Generaly there will be a " + condi + " condition."
                                });

                                replyMessage.Attachments.Add(new Attachment()
                                {
                                    Title = "Tomorrow weather forecast of " + city,
                                    ThumbnailUrl = "http://windowsgeek.lk/dpm/forecast.jpg",
                                    // TitleLink = "https://en.wikipedia.org/wiki/Bender_(Futurama)",
                                    //ThumbnailUrl = imgurl,

                                    Text = "Lowest temperature will be " + lowtemp + " °F" + " and highest temperature will be " + hightemp + "°F",
                                    FallbackText = "Lowest temperature will be " + lowtemp + " °F" + " and highest temperature will be " + hightemp + "°F"
                                });



                                return replyMessage;







                                //return message.CreateReplyMessage("Weather " + title + "\n" +
                                //" Mainly " + condition + " condition will be there." + " wind speed at " + speed + " mph. Temperature is " + temp + " °F" + "." +
                                // " Humidity is " + humidity + "%" + ". Sunrise in " + city + " at " + sunrise + " and Sunset at " + sunset + "." + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" +
                                // " Oh wait! tomorrow weather forecast of " + city + " is also available." +
                                // " Generaly there will be a " + condi + " condition. Lowest temperature will be " + lowtemp + " °F" + " and highest temperature will be " + hightemp + "°F");











                            }

                        }







                        else
                        {

                            //return message.CreateReplyMessage("Sorry. I could not find a weather report for " + message.Text + ". Can you try again with another nearby location." + "\n");



                            replyMessage.Attachments = new List<Attachment>();

                            replyMessage.Attachments.Add(new Attachment()
                            {
                                Title = "Not Found!",
                                // TitleLink = "https://en.wikipedia.org/wiki/Bender_(Futurama)",
                                ThumbnailUrl = "http://windowsgeek.lk/dpm/not.jpg",
                                Text = "I could not find a weather report for " + message.Text + ". Can you try again with another nearby city.",
                                FallbackText = "I could not find a weather report for " + message.Text + ". Can you try again with another nearby location city."
                            });

                            return replyMessage;


                        }




                    }















                }


            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
                return message.CreateReplyMessage("Hello, I can give you the weather report of your location. If you are ready, send me your location. Make no sense? Then type 'help'");
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
                return message.CreateReplyMessage("Thank you, Have a nice day!");
            }
            else if (message.Type == "UserAddedToConversation")
            {
                return message.CreateReplyMessage("Hello, I can give you the weather report of your location. If you are ready, send me your location. Make no sense? Then type 'help'");
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
                return message.CreateReplyMessage("Thank you, Have a nice day!");
            }
            else if (message.Type == "EndOfConversation")
            {
                return message.CreateReplyMessage("Thank you, Have a nice day!");
            }

            return null;
        }
    }
}