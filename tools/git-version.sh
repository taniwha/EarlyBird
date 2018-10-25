#! /bin/sh

set -e

path=`dirname $0`

full_version=`$path/git-version-gen --prefix v .tarball-version`
version=`echo $full_version | cut -d '-' -f 1 | sed -e 's/UNKNOWN/0.0.0/'`

sed -e "s/@FULL_VERSION@/$full_version/" -e "s/@VERSION@/$version/" AssemblyInfo.in > AssemblyInfo.cs-

cmp -s AssemblyInfo.cs AssemblyInfo.cs- || mv AssemblyInfo.cs- AssemblyInfo.cs

rm -f *.cs-

MAJOR=`echo $full_version | cut -f 1 -d .`
MINOR=`echo $full_version | cut -f 2 -d .`
PATCH=`echo $full_version | cut -f 3 -d .`
BUILD=`echo $full_version | cut -f 4 -d . | cut -f 1 -d '-'`
if test -z "$BUILD"; then
	BUILD=0
fi

set `head -20 $KSPDIR/readme.txt | grep ^Version | sed -e 's/\./ /g'`
KSPMAJOR=$2
KSPMINOR=$3
KSPPATCH=$4

cat > EarlyBird.version <<EOF
{
	"NAME":"EarlyBird",
	"URL":"http://taniwha.org/~bill/EarlyBird.version",
	"DOWNLOAD":"http://taniwha.org/~bill/EarlyBird_v$full_version.zip",
	"VERSION":{"MAJOR":$MAJOR,"MINOR":$MINOR,"PATCH":$PATCH,"BUILD":$BUILD},
	"KSP_VERSION_MIN":{"MAJOR":1,"MINOR":3,"PATCH":1},
	"KSP_VERSION_MAX":{"MAJOR":1,"MINOR":5,"PATCH":99}
}
EOF
#	"KSP_VERSION_MAX":{"MAJOR":$KSPMAJOR,"MINOR":$KSPMINOR,"PATCH":$KSPPATCH}
