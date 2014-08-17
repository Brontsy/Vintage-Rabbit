using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Common.Constants;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Products.Helpers;

namespace Vintage.Rabbit.Web.Models.Payment
{
    public class InvitationViewModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}")]
        [Required(ErrorMessage = "Please choose the date of the party")]
        public DateTime? PartyDate { get; set; }

        [Required(ErrorMessage = "Please enter childs name")]
        public string ChildsName { get; set; }

        [Required(ErrorMessage = "Please enter the age of the child")]
        public string Age { get; set; }

        [Required(ErrorMessage = "Please enter the time and date of the party")]
        public string PartyTime { get; set; }

        [Required(ErrorMessage = "Please enter the address of the party")]
        public string PartyAddress { get; set; }

        public string RSVPDetails { get; set; }

        public string InvitationImage { get; set; }

        public InvitationViewModel() { }

        public InvitationViewModel(Order order, Vintage.Rabbit.Parties.Entities.Party party)
        {
            var invitation = order.Items.FirstOrDefault(o => ProductHelper.IsCustomisableInvitation(o.Product));

            if(invitation != null && invitation.Product is IProduct && (invitation.Product as IProduct).Images.Any())
            {
                this.InvitationImage = (invitation.Product as IProduct).Images.First().SecureUrl;
            }

            if(party != null)
            {
                this.ChildsName = party.ChildsName;
                this.Age = party.Age;
                this.PartyDate = party.PartyDate;
                this.PartyTime = party.PartyTime;
                this.PartyAddress = party.PartyAddress;
                this.RSVPDetails = party.RSVPDetails;
            }
        }
    }
}