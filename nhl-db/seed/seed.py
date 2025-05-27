from importlib import import_module
from config import APP_ENV
from config import MIGRATION_VERSION
from modules import __all__ as modules

def parse(version):
    if version is None:
        return None
    return tuple(int(x) for x in version.split("."))

if APP_ENV != "dev":
    print("APP_ENV != dev")
    exit(0)

if MIGRATION_VERSION == "latest":
    current_migration_version = None
else:
    current_migration_version = parse(MIGRATION_VERSION)

print(f"Seeding started for migration version {MIGRATION_VERSION}")

for name in modules:
    print(f"Module {name}:")
    
    module = import_module(f"modules.{name}")

    required_migration_version_field = getattr(module, "MIGRATION", None)
    required_migration_version = parse(required_migration_version_field)

    if required_migration_version is None or current_migration_version is None or required_migration_version <= current_migration_version:
        print("     Processing")
        module.seed()
        print(f"     Finish processing module {name}")
    else:
        print(f"     Module was skipped: required version is {required_migration_version_field}")

print("All done.")