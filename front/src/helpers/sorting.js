import { useMemo } from "react";
import { useSearchParams } from "react-router-dom";

export function useSorting() {
  const [searchParams, setSearchParams] = useSearchParams();

  const sorting = useMemo(() => {
    if (searchParams && searchParams.get("o")) {
      const o = searchParams.get("o").split("-");
      return [{ id: o[0], desc: o.length > 1 && o[1] === "desc" }];
    } else {
      return null;
    }
  }, [searchParams]);

  const updateSorting = function (updater) {
    const updated = new URLSearchParams(searchParams);

    const newSorting = updater(sorting);
    if (newSorting && newSorting.length > 0) {
      const id = newSorting[0].id;
      updated.set("o", newSorting[0].desc ? `${id}-desc` : id);
    } else {
      updated.delete("o");
    }

    setSearchParams(updated);
  };

  return [sorting, updateSorting];
}
