Feature: SonicBulletHitsAnyValidTargetInPath
	Sonic bullet can hit other target that gets in path while in flight

@mytag
Scenario: Hit any target in flight path
	Given Sonic Tower fires a bullet to target One
	And There is target Two in bullets path to target One
	When Bullet reaches target Two or end of flight path range
	Then Bullet should hit target Two
