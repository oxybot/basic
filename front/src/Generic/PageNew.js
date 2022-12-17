import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { addWarning } from "../Alerts/slice";
import { apiFetch } from "../api";
import EntityForm from "./EntityForm";

const defaultOnCreate = () => {};
const defaultExtendedForm = () => null;

export default function PageNew({
  definition,
  baseApiUrl,
  texts,
  onCreate = defaultOnCreate,
  extendedForm = defaultExtendedForm,
}) {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [entity, setEntity] = useState({});
  const [errors, setErrors] = useState({});
  texts["form-action"] = "Create";

  useEffect(() => {
    if (definition) {
      let updated = { ...entity };
      definition.fields.forEach((field) => {
        if (field.type === "boolean") {
          updated[field.name] = false;
        }
      });

      setEntity(updated);
    }
  }, [definition, entity]);

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setEntity({ ...entity, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    console.log(entity);
    apiFetch(baseApiUrl, {
      method: "POST",
      body: JSON.stringify(entity),
    })
      .then(() => {
        navigate("./..");
        onCreate();
      })
      .catch((err) => {
        if (typeof err === "string") {
          dispatch(addWarning("Invalid information", "Please review the values provided"));
        } else {
          setErrors(err);
        }
      });
  };

  return (
    <div className="sticky-top">
      <EntityForm
        definition={definition}
        entity={entity}
        texts={texts}
        errors={errors}
        handleChange={handleChange}
        handleSubmit={handleSubmit}
        container
      >
        {extendedForm(entity, setEntity, errors)}
      </EntityForm>
    </div>
  );
}
