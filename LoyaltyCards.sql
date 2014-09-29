use VintageRabbit


go


select * from VintageRabbit.LoyaltyCards

Alter Table VintageRabbit.LoyaltyCards Add Title nvarchar(256)
Alter Table VintageRabbit.LoyaltyCards Add LoyaltyCardType nvarchar(256)
Alter Table VintageRabbit.LoyaltyCards Add OrderGuid uniqueIdentifier
Alter Table VintageRabbit.LoyaltyCards Add OrderGuid uniqueIdentifier
Alter Table VintageRabbit.LoyaltyCards Drop Column description

Update VintageRabbit.LoyaltyCards set Title = 'Test Loyalty Card'