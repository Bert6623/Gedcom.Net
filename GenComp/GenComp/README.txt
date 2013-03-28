GenComp 1.1

New in this version (1.1.7)

Added another case that the SNP error detection code handles. Now an SNP
	within a matching segment that does not itself match (one person is
	homozygous for one allele and the other person is homozygous for
	the other allele) will be marked as a no call in the phased data
	for the person being phased. These SNPs are spurious mismatches that
	get "forgiven" during the identification of matching segments. Setting
	these to no calls in the phased data should keep them from blocking
	the matching of smaller segments when comparing to other people. 
Added history comments to the top of phased files summarizing the 
	phasings that have been used to create the phased file.
Added detailed history companion file showing the segments used to
	phase the associated phased file. This feature is turned on or
	off using the WriteDetailedPhaseHistory setting in the config
	file. By default, this setting is false. To turn this feature
	on, edit the config file.
Made minor optimizations to reading/writing files.
Since phased and split files created by GenComp are all written in the
	tab delimited format used by 23andMe (regardless of the type of 
	the original full genome file), these files are now always given
	the .txt extension.

Instructions

Extract the files into a folder. It is easiest if you copy your raw genome
files into this same folder. There is a config file where you can specify
a different folder as the location where genome data files should be loaded
if you want to do it that way. But I prefer to just put everything in one
folder. You'll be typing the names of your genome files as parameters to
the GenComp program, so you might want to rename the files to have fairly
short names

Open a Windows command window and change your working directory to be the
folder where you've put GenComp. If you just type
GenComp 
and hit return, a short help text will be displayed. GenComp supports 
several commands. Most of the commands take genome file names as parameters.
GenComp will often need to read the original genome files, but it does
most of its work using a phased file that it creates for each genome file.
It always names these files the same as the regular genome file they are
associated with, but prepended with "phased-". Initially, these phased
files contain just the SNPs for which the person has homozygous alleles.
These homozygous alleles are trivially phased because each pair of 
chromosomes contain the same allele values. GenComp always expects you
to use the names of the original genome files for its arguments. It will
automatically take care of finding the assocated "phased-" files, and will
create them if they haven't been created yet.

The key command is MatchPhase. You can run this multiple times for a 
particular genome file, each time MatchPhasing it against a different
genome file. Each time the GenComp program will incrementally be building
up the contents of the phased file with additional SNPs it's able to 
add with each subsequent MatchPhase. 

The program does segment matching using just the phased files. The phased
files have an A side and a B side representing the two phased chromosome
pairs. When matching, it compares person 1 (P1) and person 2 (P2) four 
differnt ways. It compares P1 A to P2 A, P1 A to P2 B, P1 B to P2 A and
P1 B to P2 B.

Random Notes

This is a beta version, so there are some issues that haven't been 
addressed yet. The following notes outline some of them.

The config file contains various threshold values and some comments about
them. You can change them, but do it with consideration of the effects.

I'm not handling orientation issues right now, so if you are using FTDNA
files that are not recent, you should get them again, as FTDNA recently
changed the orientation of the genome files you download. I will add code
in the future to help deal with the non-standard orientation mess.

There is a problem I don't currently deal with at all. Initially the 
phased file contains only homozygous alleles, so the A side is identical
to the B side, i.e. no way to tell the mother side from the father side.
If I matchPhase person 1 with person 2 and get a phasing in the region 
1,000,000-10,000,000, that phasing will put the common new alleles on the
A side by default. By chance, 1 is related to 2 through her father. Then
person 1 is matchPhased with person 3 who matches on the same chromosome
at 20,000,000-30,000,000. Again, by default the phasing of that region 
assigns the shared alleles to the A side by default since in that region
the A side and B side are indistinguishable when the matchPhase is done. 
By chance, 1 is related to 3 through her mother. So really, the common 
alleles should have gone to the B side, but there is not enough info to 
make the right decision. The problem surfaces when person 1 is matched 
with person 4, who in reality shares a segment from 5,000,000-25,000,000. 
Phased matching will likely only see the segment from 5,000,000-20,000,000, 
and miss the 20,000,000-25,000,000 because it is on the wrong side. I 
currently don't handle switch errors like that. I may add switch error 
handling, but actually realigning the phased data to be correct is a much 
tougher nut because there is no source of truth.

Related to the above problem, it is dangerous to phase a person using the 
genome of a direct descendant. All the crossover points that occurred between 
person 1 and their descendant can't be recognized, and will result in person 
1 having switch errors (twists, really) at every point where a crossover 
occurred in a segment.

The MatchPhase only phases the first of the two specified genomes, using 
the data from what the two match in common. To phase both, you'd have to 
switch number 1 with number 2 and do another matchPhase. The reason I did it 
this way was because of the descendant issue mentioned above. Because of that 
problem, it may not always be desirable to phase both people when doing a 
MatchPhase of two people. If you do want to do a two way MatchPhase that 
phases both people, use the MatchPhaseBoth command.

Match start and end points will likely disagree with all the other tools 
including the previous version of GenComp. That is because segments will 
only start on the first phased match (which initially means on the first 
common homozygous allele), and will end on the last phased match. Loose 
unphased heterozygous alleles at the very start and end of a segment are 
usually included in all the other tools, but the likliehood of these wild 
card matches being true starts at 50/50 and quickly deteriorates from there 
as the length of the heterozygous run increases. So I don't use them anymore. 
Usually, since the ratio of homozygous to heterozygous snp alleles ranges 
from 1/2 to 1/4, this will only result in a discrepancy of a few SNPs.

The current version is probably not handling the X chromosome correctly. It
will require some slight differences than the other chromosomes. I will 
address that in a future version.

This is early beta. I need to do much more testing, analyzing, optimizing, 
etc. before it is beta. The next version should also have the merge command 
implemented, as it is pretty simple. When I get it to a point where it is 
feeling fairly solid, I will work on multi-threaded batch processing, and 
maybe a more user friendly windowed front end. Available time is my main 
limitation, so I can't promise a lot.

Release History

v 1.1.7 2012/02/19
Added another case that the SNP error detection code handles. Now an SNP
	within a matching segment that does not itself match (one person is
	homozygous for one allele and the other person is homozygous for
	the other allele) will be marked as a no call in the phased data
	for the person being phased. These SNPs are spurious mismatches that
	get "forgiven" during the identification of matching segments. Setting
	these to no calls in the phased data should keep them from blocking
	the matching of smaller segments when comparing to other people. 
Added history comments to the top of phased files summarizing the 
	phasings that have been used to create the phased file.
Added detailed history companion file showing the segments used to
	phase the associated phased file. This feature is turned on or
	off using the WriteDetailedPhaseHistory setting in the config
	file. By default, this setting is false. To turn this feature
	on, edit the config file.
Made minor optimizations to reading/writing files.
Since phased and split files created by GenComp are all written in the
	tab delimited format used by 23andMe (regardless of the type of 
	the original full genome file), these files are now always given
	the .txt extension.

v 1.1.6 2012/02/11
Added feature that will fill in no calls where possible, using the allele
	from the person being used to phase the target. This is enabled by
	default, but can be turned on or off in the config file.
Modified the way segments are phased if they start at the beginning of a
	chromosome, or stop at its end. If the segment is large (twice the
	minimum size for using segments for phasing), then the normal ignoring
	of the edge of the segment for phasing is skipped for the segment
	edge that aligns with the chromosome end. Without this, the beginning
	and the end of each chromosome would never get phased, even when phasing
	using parents.
Added feature that detects cases where phasing the same SNP using another
	person results in an impossible combination. For instance, if a child
	is AC and father is AA, this will phase the C to the maternal side. 
	If the child is then phased with the mother, and the mother is AA,
	that is in conflict with the C assigned to that side. Clearly, if
	the child is AC and both parents are AA, then one of the three persons
	has an error in the data for that SNP. When these are detected,
	GenComp will assign a double no call to the target's phased data for
	that SNP. Once a phased SNP is assigned the double no call, it will
	be ignored in future matching and phasing since the data is questionable.
	But doing this will avoid having that bad SNP thwart matching with
	others.
Added code to better interpret filenames entered on the command line as
	regards the inclusion or exclusion of the "phased-" prefix.

v 1.1.5 2012/02/09
Added Split command that will create two files - one containing just the
	alleles of the A side, and another containing just the B side alleles,
	padded out with all the SNPs present in the original full genome file
	and unphased alleles denoted by a special character defined in the 
	config file. Split files will have filenames prefixed with 'phasedA-'
	and 'phasedB-'.
Modified the SplitClone command to use filenames prefixed with 'phasedAA-'
	and 'phasedBB-' to differentiate them from the new Split files that
	don't contain cloned alleles.
Fixed some issues with X chromosome handling - files written by GenComp 
	will output 'X' instead of '23' as it did previously, male single
	allele X chromosome SNPs will be treated as homozygous now to
	facilitate matching and phasing (because it doesn't really know 
	whether the A side or the B side is the paternal side), so X
	chromosome matching and phasing should work better now than it did
	in previous versions.

v. 1.1.4
Fixed some bugs in matchPhase that improve the identification of matching
    segments, and assignment of phasing to one side or the other when
    matchPhasing cumulatively against several relatives related on a mix
    of maternal and paternal sides.

v. 1.1.3
Added MatchPhaseBoth command.
Added cross matching multiple files to the Match command.
Added inclusion of all phaseSide information to Match results file.
Multiple identical match segments that differ only on the phaseSide
  are now combined to a single line.
Modified some of the default/recommended configuration value.
