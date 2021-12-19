import { useEffect, useState } from "react";
import { getDefinition } from "../api";
import { groupBy, objectMap } from "../helpers";

export default function ClientDetail({ entity }) {
  const [definition, setDefinition] = useState(null);

  useEffect(() => {
    getDefinition("Client")
      .then((definition) => setDefinition(definition))
      .catch((err) => console.log(err));
  }, []);

  return (
    <>
      {definition &&
        objectMap(
          groupBy(definition.fields, (x) => x.group),
          (fields, group, index) => (
            <div key={index} className="mb-3">
              <div className="card">
                {group !== "null" && (
                  <div className="card-header">
                    <h3 className="card-title">{group}</h3>
                  </div>
                )}
                <div className="card-body">
                  {fields.map((field, index) => (
                    <div key={index} className="mb-3">
                      <div className="small text-muted">
                        {field.displayName}
                      </div>
                      <div className="lead">{entity[field.name] || "-"}</div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          )
        )}
    </>
  );
}
