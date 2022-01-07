import { groupBy, objectMap } from "../helpers";
import EntityField from "./EntityField";

export default function EntityDetail({ definition, entity }) {
  return (
    <>
      {definition &&
        objectMap(
          groupBy(definition.fields, (x) => x.group),
          (fields, group, index) => (
            <div key={index} className="card mb-3">
              {group !== "null" && (
                <div className="card-header">
                  <h3 className="card-title">{group}</h3>
                </div>
              )}
              <div className="card-body">
                {fields.map((field, index) => (
                  <div key={index} className="mb-3">
                    <div className="field-label">{field.displayName}</div>
                    <div className={field.type !== "schedule" ? "lead" : null}>
                      <EntityField type={field.type} value={entity[field.name]} />
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )
        )}
    </>
  );
}
