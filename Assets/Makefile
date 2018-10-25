MODNAME		:= EarlyBird
KSPDIR		:= ${HOME}/ksp/KSP_linux
GAMEDATA	:= ${KSPDIR}/${MODNAME}
MODGAMEDATA	:= ${GAMEDATA}/EarlyBird

RESGEN2		:= resgen2
GMCS		:= gmcs
GMCSFLAGS	:= -optimize -warnaserror
GIT			:= git
TAR			:= tar
ZIP			:= zip

all: EarlyBird_icon.png
#	${GUI_ICONS}

info:
	@echo "Extraplanetary Launchpads Build Information"
	@echo "    resgen2:    ${RESGEN2}"
	@echo "    gmcs:       ${GMCS}"
	@echo "    gmcs flags: ${GMCSFLAGS}"
	@echo "    git:        ${GIT}"
	@echo "    tar:        ${TAR}"
	@echo "    zip:        ${ZIP}"
	@echo "    KSP Data:   ${KSPDIR}"

EarlyBird_icon.png: EarlyBird_icon.svg
	inkscape --export-width=24 --export-height=24 --export-background='#000000' --export-background-opacity=0 --export-png=$@ $^

clean:
	rm -f EarlyBird_icon.png

install: all
	mkdir -p ${MODGAMEDATA}/Textures
	cp EarlyBird_icon.png ${MODGAMEDATA}/Textures

.PHONY: all clean install
