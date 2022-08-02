import { IconLoader, IconRollercoaster } from "@tabler/icons";
import clsx from "clsx";
import { useNavigate } from "react-router-dom";
import EntityFieldView from "./EntityFieldView";

import { useState, useEffect } from "react";

var sortKey;
var sortValue = 0;


export function Sorting() {

  sortValue==1?sortValue=-1:sortValue=1;

  console.log(sortValue);
  return sortValue;
}


/*export function Sorting() {
  //const [sortKey, setSortKey] = useState("UserName");
  //const [sortValue, setSortValue] = useState(0);
  
  useEffect(() => {
    if(sortValue == 1) {
      setSortValue(-1)
    }
    else {
      setSortValue(1)
    }
    
    console.log(sortValue);

  })
  return { sortValue, sortKey }
}*/

/*
function handleChange(event) {
  const value = event.target.value;
  setSortValue(value);
}
*/

function filtered(fields) {
  if (!fields) {
    return fields;
  }
  
  return fields.filter((i) => i.type !== "key");
}

export default function EntityList({ loading, definition, entities, baseTo = null, selectedId }) {

  const navigate = useNavigate();
  const fields = filtered(definition?.fields);
  
  return (
    <div className="table-responsive">
    <button onClick={() => Sorting()}>
      sorting
    </button>
      <table className="table card-table table-vcenter text-nowrap datatable table-hover">
        <thead>
          <tr>
            {fields &&
              fields.map((field, index) => (
                <th key={index} className={clsx({ "w-1": index === 0 })} onClick={() => Sorting()}>
                  {field.displayName}
                </th>
              ))}
          </tr>
        </thead>
        <tbody>
          <tr className={loading ? "" : "d-none"}>
            <td colSpan={fields?.length}>
              <IconLoader /> Loading...
            </td>
          </tr>
          {entities.map((entity) => ( 
            <tr
              key={entity.identifier}
              className={clsx({
                "table-active": entity.identifier === selectedId,
              })}
              onClick={() => baseTo !== null && navigate([baseTo, entity.identifier].filter((i) => i).join("/"))}
            >
              {entity &&
                fields &&
                fields.map((field, index) => (
                  <td key={index}>
                    <EntityFieldView type={field.type} value={entity[field.name]} list />
                  </td>
                ))}
            </tr>
          ))}
          {!loading && entities.length === 0 && (
            <tr>
              <td colSpan={fields?.length}>
                <em>No results</em>
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}

// export { sortKey, sortValue };
//export Sorting;
