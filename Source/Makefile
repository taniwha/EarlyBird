export MODNAME		:= EarlyBird
export KSPDIR		:= ${HOME}/ksp/KSP_linux
export MANAGED		:= ${KSPDIR}/KSP_Data/Managed
export GAMEDATA		:= ${KSPDIR}/GameData
export MODGAMEDATA	:= ${GAMEDATA}/${MODNAME}
export PLUGINDIR	:= ${MODGAMEDATA}/Plugins

TARGETS		:= ${MODNAME}.dll

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
	@../tools/git-version.sh

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
		-r:UnityEngine,UnityEngine.UI,UnityEngine.CoreModule,UnityEngine.IMGUIModule,UnityEngine.InputLegacyModule,UnityEngine.AnimationModule \
		-out:$@ $^

clean:
	rm -f ${TARGETS} AssemblyInfo.cs ${MODNAME}.version

install: all
	mkdir -p ${PLUGINDIR}
	cp ${TARGETS} ${MODNAME}.version ${PLUGINDIR}
#	cp ${DOC_FILES} ${MODGAMEDATA}

.PHONY: all clean install
