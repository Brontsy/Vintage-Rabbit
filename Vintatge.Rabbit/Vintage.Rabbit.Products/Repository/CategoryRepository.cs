using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Messaging.Messages;

namespace Vintage.Rabbit.Products.Repository
{
    internal interface ICategoryRepository
    {
        IList<Category> GetCategories();

        Category GetCategory(string name, ProductType productType);
    }

    internal class CategoryRepository : ICategoryRepository
    {

        public CategoryRepository()
        {
        }

        public IList<Category> GetCategories()
        {
            Category partySupplies = this.GetPartySupplies();
            Category gifts = this.GetGifts();
            Category games = this.GetGames();
            Category gamesHire = this.GetGamesHire();
            Category tablesAndChairs = this.GetTablesAndChairs();
            Category photoBooth = this.GetPhotoBooth();
            Category decorations = this.GetDecorations();
            Category glassware = this.GetGlassware();
            Category props = this.GetProps();
            Category backdrops = this.GetBackdrops();


            Category balloons = this.GetBalloons();
            Category onTheTable = this.GetOnTheTable();
            Category invitations = this.GetInvitations();
            Category decorationsBuy = this.GetDecorationsBuy();
            Category partyBags = this.GetPartyBags();

            partySupplies.Children.Add(balloons);
            partySupplies.Children.Add(onTheTable);
            partySupplies.Children.Add(invitations);
            partySupplies.Children.Add(decorationsBuy);
            partySupplies.Children.Add(partyBags);

            return new List<Category>()
            {
                partySupplies,
                gifts,
                games,
                gamesHire,
                tablesAndChairs,
                photoBooth,
                decorations,
                glassware,
                props,
                backdrops
            };
        }

        public Category GetCategory(string name, ProductType productType)
        {
            foreach(Category category in this.GetCategories().Where(o => o.ProductType == productType))
            {
                if(category.Name == name)
                {
                    return category;
                }

                if(category.Children.Any())
                {
                    foreach (Category child in category.Children)
                    {
                        if (child.Name == name)
                        {
                            return child;
                        }
                    }
                }
            }

            return null;
        }
        private Category GetGifts()
        {
            Category gifts = new Category() 
            { 
                Id = 2, 
                Name = "gifts", 
                DisplayName = "Gifts", 
                ProductType = ProductType.Buy, 
                Description = "Are you looking for birthday present ideas or online toys? It isn’t easy to find quality gifts online in Australia. We have scoured the globe for the cutest gifts that inspire the imagination, the sought after Omm design including Matryoshka dolls, or retro lunchbox, puppets for kids and animal toys. We will also be adding to our range of sought after birthday gifts for her or for him, not just any gifts but gifts that inspire the imagination.",
                SEOTitle = "Gifts online Australia – Online toys – Birthday gifts| Vintage Rabbit",
                SEOKeywords = "omm design, matryoshka dolls, nesting dolls, animal hand puppets, vintage stamps, games, kids gifts, birthday gifts for her",
                SEODescription = "The perfect Online birthday gifts including nesting dolls, lunchbox’s, animal hand puppets and omm design. The best quality kids gift in Australia. Fast shipping at the best prices."
            };

            return gifts;
        }

        private Category GetGames()
        {
            Category games = new Category() 
            { 
                Id = 3, 
                Name = "games", 
                DisplayName = "Games", 
                ProductType = ProductType.Buy,
                Description = "Those were the days back then, our memories full of precious moments playing simple kids games with friends, skipping ropes, elastics, dominoes, pin the tail on the donkey, hopscotch and pick up sticks. Vintage Rabbit brings you fun games for kids that inspire collaboration and imaginative play.",
                SEOTitle = "Kids games– retro games– unique gifts | Vintage Rabbit",
                SEOKeywords = "games for kids, kids games, fun games, old games, retro games online, hopscotch, skipping rope, giant skipping rope, elastics, dominoes, pin the tail on the donkey, skipping pebbles, elastics, childrens party games",
                SEODescription = "Kids games for birthday gifts including dominoes, skipping ropes and elastics. The best quality kids games in Australia. Fast shipping at the best prices."
            };

            return games;
        }

        private Category GetGamesHire()
        {
            Category gamesHire = new Category() 
            { 
                Id = 14, 
                Name = "games", 
                DisplayName = "Games", 
                ProductType = ProductType.Hire,
                Description = "Entertaining children at birthday parties or entertaining guests at weddings does not need to be expensive and outrageous. Some fun vintage retro games can add that unique touch that will surprise all of your guests big or small. Potato sacks for races and quoits games are an old time favourite that can be hired from Vintage rabbit.",
                SEOTitle = "Games for hire | Vintage Rabbit",
                SEOKeywords = "",
                SEODescription = ""
            };

            return gamesHire;
        }

        private Category GetTablesAndChairs()
        {
            Category tablesAndChairs = new Category() 
            { 
                Id = 4, 
                Name = "tables-and-chairs", 
                DisplayName = "Tables and chairs", 
                ProductType = ProductType.Hire,
                Description = "Hiring tables and chairs are great ideas for kids parties, 20 little chairs can be easily hired and delivered rather than stored in your garage. Our white folding kids chairs are miniature versions of the modern folding chair with little soft cushions for little soft tushes. Childrens tables and chairs offer an affordable option for kids parties and with our range of vintage and vintage inspired items you will surely impress.  From our converted door folding table to our adjustable plastic tables and beautifully restored candy buffet table hire, there are plenty of options for hire Melbourne.",
                SEOTitle = "Tables and chairs for hire | Vintage Rabbit",
                SEOKeywords = "",
                SEODescription = ""
            };

            return tablesAndChairs;
        }

        private Category GetPhotoBooth()
        {
            Category photoBooth = new Category() 
            { 
                Id = 5, 
                Name = "photo-booth", 
                DisplayName = "Photo booth", 
                ProductType = ProductType.Hire,
                Description = "Photo booth hire Melbourne can be as simple as some unique props and a Polaroid camera. Vintage Rabbit offer many photo booth packages for DIY photo booth hire that will have your guests giggling for years to come. Children just love dressing up in grannies hats, scarves and gloves with our tea party package or cuddling Olaf while wearing custom made Elsa and Anna wigs in our Frozen inspired photo booth package. All packages come with vintage gold or white frames and a selection of props.",
                SEOTitle = "Photo booths for hire | Vintage Rabbit",
                SEOKeywords = "",
                SEODescription = ""
            };

            return photoBooth;
        }

        private Category GetDecorations()
        {
            Category decorations = new Category() 
            { 
                Id = 6, 
                Name = "decorations", 
                DisplayName = "Decorations", 
                ProductType = ProductType.Hire,
                Description = "At Vintage Rabbit we spend hours dreaming of party decoration ideas and we love every minute! We search for those modern unique items that can make a statement. Paper decorations, bunting flags, gold and pink paper tassels, and snowflakes. If you are searching for how to make tissue paper pom poms then we have the answer that will save you time, we have gorgeous pre-cut paper pom poms in stunning pastels and vibrant colours. \r\nIf you really want to impress your friends we also hire backdrops that will delight your children, have a look at our <a href=\"/hire\">hire</a> and <a href=\"/style\">theme</a> sections for more.",
                SEOTitle = "Decorations for hire | Vintage Rabbit",
                SEOKeywords = "",
                SEODescription = ""
            };

            return decorations;
        }

        private Category GetGlassware()
        {
            Category glassware = new Category() 
            { 
                Id = 7, 
                Name = "glassware-and-crockery", 
                DisplayName = "Glassware & Crockery", 
                ProductType = ProductType.Hire,
                Description = "Glassware hire Melbourne if available from Vintage Rabbit with a selection of apothecary jars, custom made woven hessian glass jars and bottles that create beautiful candy buffet tables, party decorations and kids tables. Available for hire are gorgeous mini glass milk bottles that can be hired with our authentic soft drink crate.",
                SEOTitle = "Glassware and Crockery for hire | Vintage Rabbit",
                SEOKeywords = "",
                SEODescription = ""
            };

            return glassware;
        }

        private Category GetProps()
        {
            Category props = new Category() 
            { 
                Id = 8, 
                Name = "props", 
                DisplayName = "Props", 
                ProductType = ProductType.Hire,
                Description = "Party props are a great way to add a unique element to any event without spending a fortune on decorations you may never use again. Handmade, hand sourced one of a kind items that really draw attention to the detail of your stylish event. Mini mushrooms, carnival tickets, bird cage props, handmade kites, wood cake stands, soft drink crates, Dorothy ruby slippers and model hot air balloons.",
                SEOTitle = "Props for hire | Vintage Rabbit",
                SEOKeywords = "",
                SEODescription = ""
            };

            return props;
        }

        private Category GetBackdrops()
        {
            Category backdrops = new Category() 
            { 
                Id = 14, 
                Name = "backdrops", 
                DisplayName = "Backdrops", 
                ProductType = ProductType.Hire,
                Description = "Handmade Vintage Rabbit backdrops are great ideas for kids parties to add a unique pop of colour that will impress your guests above all expectations. Looking for a Frozen themed girls birthday party that really stands out from the rest? Our custom made giant glittery “let it go” backdrop is such a unique prop hire you will stop searching for birthday party ideas and start sending out our hand drawn customisable party invitations immediately!",
                SEOTitle = "Backdrops for hire | Vintage Rabbit",
                SEOKeywords = "",
                SEODescription = ""
            };

            return backdrops;
        }

        private Category GetPartySupplies()
        {
            Category partySupplies = new Category() 
            { 
                Id = 1, 
                Name = "party-supplies", 
                DisplayName = "Party supplies", 
                ProductType = ProductType.Buy,
                Description = "We love unique, beautiful party supplies that add simple effective statements to your event, with just one or two pieces. That’s why we have created a one of a kind online party shop, dedicated to the most beautiful party supplies in Australia. The most unique kids party supplies online, we source our products from one of a kind handmade suppliers and the best quality suppliers on the market. Our range encompasses all of the items available in our packages including wooden spoon, wooden bowl/plates, birthday balloons, tissue paper flowers, kids paper snowflakes and serviettes. \r\nLooking for the whole package for your kids birthday parties? We offer full package party themes including a mixture of hire and purchase items that have been fully styled. Check out or <a href=\"/style\">theme</a> page for more details.",
                SEOTitle = "Party supplies – Party Decorations – Party Shop| Vintage Rabbit",
                SEOKeywords = "party supplies Melbourne, party supplies Australia, napkins, straws, party decorations, wooden spoon, printed napkins, serviettes, wooden bowl, party decorations, paper decorations, pom balls",
                SEODescription = "The best party supplies for any style, paper straws, paper pom poms, wooden spoon, balloons, and table decorations. The best quality kids party supplies in Melbourne and Australia. Fast shipping at the best prices."
            };

            return partySupplies;
        }

        private Category GetOnTheTable()
        {
            Category onTheTable = new Category() 
            { 
                Id = 10, 
                Name = "on-the-table", 
                DisplayName = "On the table", 
                ProductType = ProductType.Buy,
                Description = "Looking for great party ideas that can enhance your kids party table decorations? Then start with a simple wooden spoon that is either plain or gorgeously painted with modern chevron patterns in a variety of colours. Add a dash of sophistication with a disposable wooden bowl that can also be used as a plate, then match your colours with delightful paper striped straws and colourful serviettes. Let your table shine with colour, the brilliance is in the simplicity!",
                SEOTitle = "Table decorations – Party Supplies – Serviettes | Vintage Rabbit",
                SEOKeywords = "party supplies Melbourne, party supplies Australia, napkins, straws, party decorations, wooden spoon, serviettes, wooden bowl, striped straws, party ideas",
                SEODescription = "The best party supplies for any style, paper straws, wooden spoon, table decorations, and serviettes. The best quality kids party supplies in Melbourne and Australia. Fast shipping at the best prices."
            };

            return onTheTable;
        }

        private Category GetBalloons()
        {
            Category balloons = new Category() 
            { 
                Id = 9, 
                Name = "balloons", 
                DisplayName = "Balloons", 
                ProductType = ProductType.Buy,
                Description = "Vintage Rabbit kids love birthday balloons! We have a full range of party balloons in single colours that can add a touch of pastel, creating a modern flair and giving your little guest of honour all their heart desires. Pink balloons, red balloons, yellow balloons, and so many more. The most beautiful of all, are our big round balloons that come in a variety of modern colours, a big balloon, a giant balloon that can really make your childs birthday dreams come true.",
                SEOTitle = "Balloons | Vintage Rabbit",
                SEOKeywords = "",
                SEODescription = ""
            };

            return balloons;

        }
        private Category GetInvitations()
        {
            Category invitations = new Category() 
            { 
                Id = 11, 
                Name = "invitations", 
                DisplayName = "Invitations", 
                ProductType = ProductType.Buy,
                Description = "",
                SEOTitle = "Customised Invitations – Party Supplies – Printed Invitations| Vintage Rabbit",
                SEOKeywords = "Customised invites, customisable invitations, printed invitations, invitation boxes, online invitations, party invites, kids party supplies",
                SEODescription = "Hand painted customisable printed invitations and packaged invitations. The best quality kids party supplies in Melbourne and Australia. Fast shipping at the best prices."
            };

            return invitations;
        }
        private Category GetDecorationsBuy()
        {
            Category decorationsBuy = new Category() 
            { 
                Id = 12, 
                Name = "decorations", 
                DisplayName = "Decorations", 
                ProductType = ProductType.Buy,
                Description = "",
                SEOTitle = "Party decorations – Party Supplies – Paper decorations | Vintage Rabbit",
                SEOKeywords = "red balloon, party ideas, party decorations, paper decorations, paper pom poms, bunting flags, tissue paper flowers, kids snowflakes, balloons, giant balloons, latex balloons, red balloons, balloons Melbourne, balloons Sydney, helium balloons",
                SEODescription = "Paper decorations including paper pom poms, bunting flags and balloons. The best quality kids party supplies in Melbourne and Australia. Fast shipping at the best prices.."
            };

            return decorationsBuy;
        }
        private Category GetPartyBags()
        {
            Category partyBags = new Category() 
            { 
                Id = 13, 
                Name = "party-bags", 
                DisplayName = "Party Bags", 
                ProductType = ProductType.Buy,
                Description = "",
                SEOTitle = "Party Bags | Vintage Rabbit",
                SEOKeywords = "",
                SEODescription = ""
            };

            return partyBags;
        }
    }
}
