KSPDIR		:= ${HOME}/ksp/KSP_linux
MANAGED		:= ${KSPDIR}/KSP_Data/Managed
GAMEDATA	:= ${KSPDIR}/GameData
EBGAMEDATA  := ${GAMEDATA}/EarlyBird
PLUGINDIR	:= ${EBGAMEDATA}/Plugins

TARGETS		:= EarlyBird.dll

EB_FILES := \
    AssemblyInfo.cs	\
	EarlyBird.cs \
	FlightWindow.cs \
	Settings.cs \
	Sunrise.cs \
	Toolbar.cs \
	VersionReport.cs \
	$e

DOC_FILES := \
	License.txt \
	README.md

RESGEN2		:= resgen2
GMCS		:= mcs
GMCSFLAGS	:= -optimize -warnaserror
GIT			:= git
TAR			:= tar
ZIP			:= zip

all: version ${TARGETS} #EarlyBird.png

.PHONY: version
version:
	@./git-version.sh

info:
	@echo "EarlyBird Build Information"
	@echo "    resgen2:    ${RESGEN2}"
	@echo "    gmcs:       ${GMCS}"
	@echo "    gmcs flags: ${GMCSFLAGS}"
	@echo "    git:        ${GIT}"
	@echo "    tar:        ${TAR}"
	@echo "    zip:        ${ZIP}"
	@echo "    KSP Data:   ${KSPDIR}"

EarlyBird.dll: ${EB_FILES}
	${GMCS} ${GMCSFLAGS} -t:library -lib:${MANAGED} \
		-r:Assembly-CSharp,Assembly-CSharp-firstpass \
		-r:UnityEngine,UnityEngine.UI \
		-out:$@ $^

EarlyBird.png: EarlyBird.svg
	inkscape --export-png $@ $^

clean:
	rm -f ${TARGETS} AssemblyInfo.cs EarlyBird.png

install: all
	mkdir -p ${PLUGINDIR}
	cp ${TARGETS} ${PLUGINDIR}
#	cp ${DOC_FILES} ${EBGAMEDATA}

.PHONY: all clean install
