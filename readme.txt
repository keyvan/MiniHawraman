Mini Hawraman Podcast Engine
Author: Keyvan Nayyeri
Blog: http://keyvan.io
Podcast: http://keyvan.fm
Twitter: http://twitter.com/keyvan
Contact Info: http://keyvan.tel
******************************************************************
Mini Hawraman is a minimized and compact version of Hawraman
podcast engine (running on www.keyvan.fm) that is being used
on www.mashthis.io and is released as an open source project for
those who want a solution to get started with because I noticed
that there isn't such a solution out there.

Here is a list of features included in Mini Hawraman:
* RSS generator with iTunes metadata support
* 2-step verification for higher security
* Sitemap generator
* Basic search
* Archive
* Separate summary and description for each episode
* Separate field for show links
* Separate field for guest(s) name
* Separate field for guest(s) biography
* Separate field for episode length
* HTML5 player

Here is a list of features missing from original Hawraman:
* Comment, trackback, and pingback
* File uploader
* Multiple downloads for each episode with different formats
* Downloads counter
* Advanced search
* Thumbnail manager
* About, copyright, terms of use, and other content pages

In addition to these, Mini Hawraman misses the unit testing
since the structure of the code was changed and I didn't want
to spend time rewriting the tests to compile and work.

Important Note:
It's worth pointing that my main intention of releasing
this code is to provide a starter point for those who want
to have their own podcast and like me and many other
people, they're frustrated by customized blog engine solutions
for podcasting. The use of Mini Hawraman requires C# and
ASP.NET MVC programming skills even though I tried to simplify
things as much as possible. Therefore, please have the correct
mindset about this project and be ready to modify certain parts
to make it work for you, and definitely take a look at the source
code before deploying your podcast.

I'd hope to see other programmers using this codebase to make
it better and provide the first mature solution ever for
podcasting on the web.