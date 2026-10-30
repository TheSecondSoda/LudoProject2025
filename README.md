# Ludo projekt - Softwareudvikler 1. semester

## Problemformulering
Hent regler til Ludo her: https://www.papskubber.dk/braetspil/ludo Brug ikke GlobeLudo og varianter!
Få delt opgaverne op i så små “kasser” som muligt, så bliver det lettere at teste. Vis opdeling via flowdiagram. brug evt drawIO: https://app.diagrams.net/ 
Del spillet op i en frontend til visning, der læser fra API fra backend der styrer regler m.m.
Lav sprint (“uge”) med opgaver og beskrivelser på den mest supporterende måde for udvikler-teamet.
Lav unit-Test, component-test, API-test,  som skal kunne dække hele spillet. (Test driven development) frontend-test er ikke et krav.
Kodning af spillet iht. SOLID, så test bliver grøn/overholdt. hvis noget mangler i planlægningen skal der laves test først, og herefter kode, samt planlægning/sprint skal opdateres. Man kan kun ligger nye opgaver i efterfølgende sprint.

Note: en del af opgaven var netop at reglerne ikke var fyldestgørende, og at vi skulle spørge ind til eventuelle misforståelser med product owneren (underviseren).
Note: kopieret direkte fra den originale opgavebeskrivelse.

## Om projektet
Softwarekvalitets projekt på softwareudviklings uddannelsen, 2025 (første semester). Team på fem personer: Isak, Phillip, Henrik, Mathias og Thomas.

## Tech stack
- .NET 8.0 / C#
- FluentAssertions
- xUnit
- Moq
- Reqnroll
- Monogame

## Kom i gang
Backenden og frontenden skal startes separat fra hinanden på samme enhed. Backenden først, så frontenden kan få fat i den når den skal slå terninger til at starte spillet.
Hvis de to services kører på andre placeringer, udskift URL'erne i frontenden så de matcher backendens URL.

## Arkitektur
Frontend og backend har hvert deres projekt, hvor de begge følger SOLID til en overdrevet precition. Det vil sige at alt er delt op i services, domæne objekter og interfaces, alle med deres egne filer.
Unit tests og integrations tests har hver deres eget projekt.

## Status og reflektion
Helt ærligt, der er mange ting jeg gerne ville have ændret. Men man kunne også se på gruppemedlemmerne at der var megen forskel på hvor meget indsats vi var villige til at lægge i projektet, og derved var der også meget forskel på kvaliteten af forskellige dele. URL handling kunne håndteres bedre, ikke alle fulgte test-first fra starten af sådan som projektet var ment, der er stadigt bugs her og der, den slags ting. Men i forhold til at projektet er lavet af en gruppe studerende der aldrigt før har arbejdet sammen med hinanden, gik det fint for et første forsøg.