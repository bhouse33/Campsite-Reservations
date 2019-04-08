--delete tables to clean out data for testing
delete from reservation
delete from [site]
delete from campground
Delete from park

-- Insert a fake park
INSERT INTO park VALUES('Jelly Mountain', 'New Mexico', '1981-03-05', 2000, 250, 'A park of jelly mountains');
DECLARE @newParkId int = (SELECT @@IDENTITY);

insert into campground Values(@newParkId, 'Jellyground', 4, 10, 25.50);
DECLARE @newCampgroundId int = (SELECT @@IDENTITY);

insert into [site] Values(@newCampgroundId, 1, 25, 1,45,0);
DECLARE @newSiteId int = (SELECT @@IDENTITY);


insert into reservation values(@newSiteId, 'Bilbo Yoggins', '2012-05-01', '2012-05-06', GetDate());
DECLARE @newReservationId int = (SELECT @@IDENTITY);

-- Return the ids of the fake park, campground, site, and reservation
SELECT 
 @newParkId as newParkId,
 @newCampgroundId as newCampgroundId,
 @newSiteId as newSiteId,
 @newReservationId as newReservationId;
