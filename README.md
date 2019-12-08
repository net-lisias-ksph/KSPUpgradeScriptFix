# KSPUpgradeScriptFix

A hack to prevent vessel part upgrade code, introduced by squad in 1.8.0, from crashing on custom control surfaces.


## In a Hurry

* [Binaries](https://github.com/net-lisias-ksph/KSPUpgradeScriptFix/tree/Archive)
* [Sources](./Source)
* [Change Log](./CHANGE_LOG.md)


## Description

[Boris-Barboris](https://forum.kerbalspaceprogram.com/index.php?/profile/133181-boris-barboris/)
> What is that v180_ModuleControlSurface class, did they rewrite them for 1.8.0? If I derive from it instead of ModuleControlSufrace, will that help?

[Morse](https://forum.kerbalspaceprogram.com/index.php?/profile/154930-morse/)
> It's "SaveUpgradePipeline.v180_ModuleControlSurface" which deals with save files upgrading. I guess. With KSP you never know, but that's my assumption. I inherited from these classes and replaced the class name comparison with "is". Tried to not touch anything else as much as possible.
>
> But it indeed won't magically fix everything, just the NPEs in case you're upgrading your save or craft files from 1.7


## Known dependants

* [Atmosphere Autopilot](https://forum.kerbalspaceprogram.com/index.php?/topic/124417-*) on 1.8.0 <= KSP <= 1.8.1
	+ Newer KSP can have the problem fixed, and so this hack will not be needed anymore.


## References

* [Morse](https://forum.kerbalspaceprogram.com/index.php?/profile/154930-morse/)
	+ [KSP Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/124417-180-181-atmosphereautopilot-1516/&do=findComment&comment=3695094)

