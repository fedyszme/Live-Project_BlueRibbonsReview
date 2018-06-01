# Live-Project_BlueRibbonsReview
Live project with Prosper IT, working with a team of 8 other developer to develop new features and functionality for an ASP.NET MVC application using Entity Framework. Blue Ribbons Review is website where users can either sell or buy items at a discount. The users register their accounts, and can be sellers or both buyers and sellers.

Due to confidentiality I am unable to post the entire project but do have permission to post some of the individual files, code snippets and screen shots. Below is a bit more description on some of the more interesting user stories I worked on.

I worked with the Admin Controller, AdminViewModel, and Views. I created the logic for the edit, details, delete action results and created the corresponding views while altering the index. 

I created a link in the Admin Index View that creates a CSV file with the emails of the users. This utilized StringWrither to do so.

I added the functionality to the CampaignsController - Create, that when the form is submitted, it also sends an email to the support team letting them know that their is a new item for them to review.

I adjusted the Reviews Index page to use the data from the AnalyticsViewModel and made it so that it displayed a list of the products and their average review rating, and then when the product is chosen, it drops down the individual reviews of that product.

I set up the ability to register and sign in with a facebook account. 

I added the font-awesome icons, performed various styling changes and various little adjustments as well. 
