-- DECLARE @Ident int;
-- SET @Ident = 1;
-- WHILE @Ident <= 100
-- BEGIN
      
-- INSERT INTO Research VALUES(
-- 1,
-- '2017-08-28',
-- 28895,
-- '2017',
-- '2017',
-- 1,
-- 1,
-- 1,
-- 5,
-- '/bucket/4e9d6ee3-6ce5-4d81-b5ec-cec2108c2bc7/research/bd292010-69d6-4232-8413-7726d80af0f2.pdf',
-- 'Could you comment on ',
-- 'Mundial',
-- 'How big is the threat from North Korea?'+ STR(@Ident),
-- '2017-08-28');
--       SET @Ident += 1;
-- END
UPDATE TeacherArticles SET TeacherArticles.[Order] = 1 WHERE ArticleId = 777756 AND TeacherId = 'd52266ca-809e-450d-a860-8af3ca83eca4'

SELECT * FROM Articles WHERE Id = 777763
SELECT * FROM  TeacherArticles where ArticleId = 777750 
--AND TeacherId = 'd52266ca-809e-450d-a860-8af3ca83eca4'
